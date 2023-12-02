using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Xml.Linq;
using NwsToRvt.Clashes;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using RevitDevClasses.Model;
using RevitDevClasses.GenericModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace NwsToRvt
{
    public class NwsToRvt_ViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private RevitApiDataExtractor revitApiDataExtractor;

        private Document _currentDoc;
        public Document CurrentDoc
        {

            get { return _currentDoc; }

            set { _currentDoc = value; }

        }

        private List<Element> _currentDocSpheres;
        public List<Element> CurrentDocSpheres 
        {
            get { return _currentDocSpheres; }
            set { _currentDocSpheres = value; }
        }

        private ClashControler _clashControler;

        private string _XMLfilePath;
        public string XMLfilePath 
        { 
            get { return _XMLfilePath;}
            set { _XMLfilePath = value; }
        }


        private XDocument _selectedXMLDocument;
        public XDocument SelectedXMLDocument 
        {
            set 
            { 
                _selectedXMLDocument = value; 

                this.Clashes = ClashReportReader.ExtractClashes(this.SelectedXMLDocument);

                foreach (NavisWorksClash clash in this.Clashes) 
                {
                    //if (_clashControler.IsClashMarked(clash)) clash.IsMarked = true;

                    clash.IsMarked = _clashControler.IsClashMarked(clash);

                }
            }

            get { return _selectedXMLDocument; }
        }


        private ObservableCollection<NavisWorksClash> _clashes = new ObservableCollection<NavisWorksClash>();
        public ObservableCollection<NavisWorksClash> Clashes 
        {
            get { return _clashes; }
            set 
            {

                var sortedClashes = value.OrderBy(clash => clash.Number).ToList();

                foreach (var sortedClash in sortedClashes)
                {
                    _clashes.Add(sortedClash);
                }

                OnPropertyChanged();
            }      
        }


        private List<string> _clashInformation;
        public List<string> ClashInformation 
        {     
            get { return _clashInformation; }
            set { _clashInformation = value; }
        }   



        public NwsToRvt_ViewModel(RevitApiDataExtractor rvtApiDataExtractor) 
        {
            this.revitApiDataExtractor = rvtApiDataExtractor;
            this.CurrentDoc = rvtApiDataExtractor.GetCurrentDocument();
            this._clashControler = new ClashControler(this.CurrentDoc);
        }

        public void MarkNewClashes() 
        {
            this._clashControler.MarkNewClashes(this.Clashes);
        }

        public void MarkActiveClashes()
        {
            this._clashControler.MarkActiveClashes(this.Clashes);
        }

        public void DeleteObsoleteClashMarkings()
        {
            this._clashControler.DeleteObsoleteClashMarkings(this.Clashes);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // when the CurrentDocument_LinkedDocuments property is set, this line raises the event "PropertyChanged". This event has been created once we applied the INotifyPropertyChanged interface to the Viewmodel class
            // The "?" checks wether the PropertyChanged event has any suscribers. Meaning : should any parts of the code be notified that the ViewModel's field changed.
            // The suscribers are all the parts of the View code which are bound to the ViewModel class' fields.
            // Invoke is the method that raises the event. It takes two arguments : 
            //   First : The object that is raising the event. In this case : The ViewModel itself.
            //   Second : The information about which property of the object has changed. The PropertyChangedEventArgs gives this information.
            // Once The PropertyChanged event has been raised, a notification is sent to its suscriber : The View. The View will then updates the data it shows, using the binding.
        }

    }
}
