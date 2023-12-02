using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using NwsToRvt.Clashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace NwsToRvt.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private NwsToRvt_ViewModel _selectedViewModel;

        public MainWindow(NwsToRvt_ViewModel viewModel)
        {

            this._selectedViewModel = viewModel;
            this.DataContext= _selectedViewModel;
            InitializeComponent();

        }

        private void SelectXML(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Fichiers XML | *.xml";

            bool? success = fileDialog.ShowDialog();

            if (success == true) 
            {      
                string xmlFilePath = fileDialog.FileName;
                XDocument selectedXMLFile = XDocument.Load(xmlFilePath);
                this._selectedViewModel.SelectedXMLDocument = selectedXMLFile;
            }
        }

        private void UpdateClashMarkings(object sender, RoutedEventArgs e) 
        {
            this._selectedViewModel.MarkNewClashes();
            this._selectedViewModel.MarkActiveClashes();
            this._selectedViewModel.DeleteObsoleteClashMarkings();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ScrollViewer1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Update the VerticalOffset of the other two ScrollViewer controls
            scrollViewer2.ScrollToVerticalOffset(e.VerticalOffset);
            scrollViewer3.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void ScrollViewer2_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Update the VerticalOffset of the other two ScrollViewer controls
            scrollViewer1.ScrollToVerticalOffset(e.VerticalOffset);
            scrollViewer3.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void ScrollViewer3_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Update the VerticalOffset of the other two ScrollViewer controls
            scrollViewer1.ScrollToVerticalOffset(e.VerticalOffset);
            scrollViewer2.ScrollToVerticalOffset(e.VerticalOffset);
        }

    }
}
