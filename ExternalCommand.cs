using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NwsToRvt.UI;
using RevitDevClasses.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NwsToRvt
{

    [TransactionAttribute(TransactionMode.Manual)]

    public class ExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            RevitApiDataExtractor RVTAPIDataExtractor = new RevitApiDataExtractor(commandData);

            NwsToRvt_ViewModel viewModel = new NwsToRvt_ViewModel(RVTAPIDataExtractor);

            MainWindow mainWindow = new MainWindow(viewModel);
            mainWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}
