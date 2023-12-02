using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using System.Xml.Linq;
using System.Xml;
using NwsToRvt.ConvertUtils;
using System.ComponentModel;

namespace NwsToRvt.Clashes
{
    public class NavisWorksClash : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        private string _name;
        public string Name 
        {
            get { return _name; }
            set { _name = value; }
        }


        private int _number;

        public int Number
        
        {
        
            get 
            {
                //return _name

                return int.Parse (_name.Substring("Conflit".Length));



                ; 
            
            
            }
        
        
        
        }



        private XYZ _coordinates;
        public XYZ Coordinates 
        {      
            get { return _coordinates; }
            set { _coordinates = value; }       
        }

        private string _status;

        public string Status 
        {     
            set 
            { 
                _status = value;
                Status_FrenchLang = value;
            }
            get { return _status; }
              
        }    


        private string _status_FrenchLang;
        public string Status_FrenchLang 
        {
            get { return _status_FrenchLang; }
            set 
            {
                if (value == "new") _status_FrenchLang = "Nouveau";
                if (value == "active") _status_FrenchLang = "Actif";
                if (value == "resolved") _status_FrenchLang = "Résolu";
            }     
        }


        private bool _isMarked;
        public bool IsMarked 
        {           
            get { return _isMarked; }
            set 
            { 
                _isMarked = value;
                if (value == true) IsMarked_FrenchLang = "Repéré";
                if (value == false) IsMarked_FrenchLang = "Non repéré";
            }       
        }

        
        private string _isMarked_FrenchLang;


        public string IsMarked_FrenchLang 
        {
            get { return _isMarked_FrenchLang; }
            set 
            { 
                _isMarked_FrenchLang = value ; 
                OnPropertyChanged(nameof(IsMarked_FrenchLang));  
            }
        }

        public NavisWorksClash(XElement clashAsXMLData, string navisworksReportUnit) 
        {

            List<double> clashCoordinates = new List<double>();


            string XValue_AsString = clashAsXMLData.Element("clashpoint").Element("pos3f").Attribute("x")?.Value;
            XValue_AsString = XValue_AsString.Replace(".", ",");
            clashCoordinates.Add(Convert.ToDouble(XValue_AsString));

            string YValue_AsString = clashAsXMLData.Element("clashpoint").Element("pos3f").Attribute("y")?.Value;
            YValue_AsString = YValue_AsString.Replace(".", ",");
            clashCoordinates.Add(Convert.ToDouble(YValue_AsString));


            string ZValue_AsString = clashAsXMLData.Element("clashpoint").Element("pos3f").Attribute("z")?.Value;
            ZValue_AsString = ZValue_AsString.Replace(".", ",");
            clashCoordinates.Add(Convert.ToDouble(ZValue_AsString));


            if (navisworksReportUnit == "m")
            {
                clashCoordinates = clashCoordinates.Select(coordinateValue => ConvertToFeetUtils.ConvertMetersToFeet(coordinateValue)).ToList();
            }

            if (navisworksReportUnit == "cm")
            {
                clashCoordinates = clashCoordinates.Select(coordinateValue => ConvertToFeetUtils.ConvertCentimetersToFeet(coordinateValue)).ToList();
            }

            if (navisworksReportUnit == "mm")
            {
                clashCoordinates = clashCoordinates.Select(coordinateValue => ConvertToFeetUtils.ConvertMillimetersToFeet(coordinateValue)).ToList();
            }




            this.Coordinates = new XYZ(clashCoordinates[0], clashCoordinates[1], clashCoordinates[2]);
            this.Name = clashAsXMLData.Attribute("name")?.Value;
            this.Status= clashAsXMLData.Attribute("status")?.Value;




        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
