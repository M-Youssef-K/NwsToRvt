using System;
using System.Reflection;
using System.Windows.Media.Imaging;



using Autodesk.Revit.UI;


namespace NwsToRvt
{
    internal class Application : IExternalApplication
    {

        public Result OnStartup(UIControlledApplication application)
        {
            string Assembly_Name = Assembly.GetExecutingAssembly().Location;
            string Assembly_Path = System.IO.Path.GetDirectoryName(Assembly_Name);

            string RibbonTab_Name = "NwsToRvt";
            string RibbonPanel_Name = "NwsToRvt";

            application.CreateRibbonTab(RibbonTab_Name);
            RibbonPanel RibbonPannel = application.CreateRibbonPanel(RibbonTab_Name, RibbonPanel_Name);

            PushButtonData Button = new PushButtonData("NwsToRvt", "NwsToRvt", Assembly_Name, "NwsToRvt.ExternalCommand");

            Button.LargeImage = new BitmapImage(new Uri(Assembly_Path + @"\img\AppIcon.png"));

            RibbonPannel.AddItem(Button);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Failed;

        }
    }
}
