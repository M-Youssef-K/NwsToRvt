using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

using Autodesk.Revit.DB;
using System.Collections.ObjectModel;

namespace NwsToRvt.Clashes
{
    internal class ClashReportReader
    {


        public static string ExtractUnit(XDocument XMLDocument) 
        {
            XElement batchTest = XMLDocument.Descendants("batchtest").FirstOrDefault();
            return (string)batchTest.Attribute("units") ;     
        }

        public static ObservableCollection<NavisWorksClash> ExtractClashes(XDocument XMLDocument)
        {

            string navisworksReportUnit = ClashReportReader.ExtractUnit(XMLDocument);

            ObservableCollection<NavisWorksClash> NavisWorksClashes = new ObservableCollection<NavisWorksClash>();
            IEnumerable<XElement> ClashesAsXMLData = XMLDocument.Descendants("clashresult");

            foreach (XElement clashAsXMLData in ClashesAsXMLData) 
            {
                NavisWorksClash NWSclash = new NavisWorksClash(clashAsXMLData , navisworksReportUnit);


                NavisWorksClashes.Add(NWSclash);
            }

            return NavisWorksClashes;
        }
    }
}
