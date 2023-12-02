using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitDevClasses.GenericModels;
using RevitDevClasses.Enum;
using System.Collections.ObjectModel;

namespace NwsToRvt.Clashes
{
    public class ClashControler
    {
        private Document _currentDoc;
        private  IList<Element> CurrentDoc_FamilySymbols;
        private FamilySymbol Spheres_FamilySymbol;

        private static string _sphereFamilyName = "Reperage3D";
        private static string _sphereFamilySymbolName = "Reperage3D_Sphere";

        private List<Element> _currentDocSpheres;
        public  List<Element> CurrentDocSpheres 
        {   
            get { return _currentDocSpheres; }
            set { _currentDocSpheres = value;}
  
        }

        public ClashControler(Document currentDoc) 
        {
            FamilySymbol GetSphereFamilySymbol(IList<Element> CurrentDoc_FamilySymbols, string FamilySymbolName)
            {
                foreach (FamilySymbol FamSymbol in CurrentDoc_FamilySymbols)
                {
                    if (FamSymbol.Name == FamilySymbolName) return FamSymbol;
                }
                return null;
            }


            this._currentDoc = currentDoc;

            CurrentDoc_FamilySymbols = new FilteredElementCollector(_currentDoc).OfClass(typeof(FamilySymbol)).ToElements();
            this.Spheres_FamilySymbol = GetSphereFamilySymbol(CurrentDoc_FamilySymbols, _sphereFamilySymbolName);

            IList<Element> CurrentDoc_AllGenericModels = new FilteredElementCollector(_currentDoc).OfCategory(BuiltInCategory.OST_GenericModel).WhereElementIsNotElementType().ToElements();
            this.CurrentDocSpheres = GenericModelFilter.FilterGenericModelsByFamilyName(CurrentDoc_AllGenericModels, _sphereFamilyName, SortByNameOptions.NameEqualsValue);
        }


        public bool IsClashMarked(NavisWorksClash clash)
        {
            foreach (Element selectedSphere in this.CurrentDocSpheres)
            {
                XYZ selectedSphere_CentralPoint = (selectedSphere.Location as LocationPoint).Point;
                if (clash.Coordinates.DistanceTo(selectedSphere_CentralPoint) < 0.05) return true;
            }
            return false;
        }

        public void MarkNewClashes(ObservableCollection<NavisWorksClash> clashes) 
        {
            Transaction DocTransaction = new Transaction(_currentDoc, "Repérage des conflits nouveaux");
            DocTransaction.Start();

            foreach (NavisWorksClash clash in clashes) 
            {
                if (clash.Status == "new") 
                {
                    if (IsClashMarked(clash)) continue;



                    FamilyInstance NewSphere = _currentDoc.Create.NewFamilyInstance(clash.Coordinates, Spheres_FamilySymbol, StructuralType.NonStructural);

                    clash.IsMarked = true;
                }
            }
            DocTransaction.Commit();
        }




        public void MarkActiveClashes(ObservableCollection<NavisWorksClash> clashes) 
        {
            Transaction DocTransaction = new Transaction(_currentDoc, "Repérage des conflits actifs");
            DocTransaction.Start();

            foreach (NavisWorksClash clash in clashes)
            {
                if (clash.Status == "active")
                {
                    if (IsClashMarked(clash)) continue;
                    FamilyInstance NewSphere = _currentDoc.Create.NewFamilyInstance(clash.Coordinates, Spheres_FamilySymbol, StructuralType.NonStructural);
                }
            }
            DocTransaction.Commit();
        }




        public void DeleteObsoleteClashMarkings(ObservableCollection<NavisWorksClash> clashes) 
        {
            List<ElementId> SpheresToDeleteIDs = new List<ElementId>();

            foreach (NavisWorksClash clash in clashes)
            {
                if (clash.Status == "resolved")
                {
                    foreach (Element selectedSphere in this.CurrentDocSpheres) 
                    {
                        XYZ selectedSphere_CentralPoint = (selectedSphere.Location as LocationPoint).Point;

                        if (clash.Coordinates.DistanceTo(selectedSphere_CentralPoint) < 0.1)
                        {
                            clash.IsMarked = false;
                            SpheresToDeleteIDs.Add(selectedSphere.Id);

                        }
                    }
                }
            }

            Transaction DocTransaction = new Transaction(_currentDoc, "Suppression des marquages obsoletes");
            DocTransaction.Start();

            foreach (ElementId selectedSphereId in SpheresToDeleteIDs) 
            {     
                this._currentDoc.Delete(selectedSphereId);
            }

            DocTransaction.Commit();
        }


    }
}
