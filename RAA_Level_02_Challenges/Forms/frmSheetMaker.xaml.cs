using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Autodesk.Revit.DB;
using Microsoft.Win32;

namespace RAA_Level_02_Challenges
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class frmSheetMaker : Window
    {
        ObservableCollection<SheetData> sheetList {  get; set; }
        ObservableCollection<Element> TBlockData { get; set; }
        ObservableCollection<View> ViewData { get; set; }

        public List<Element> elemList;

        public frmSheetMaker(List<Element> TblockList, List<View> ViewList)
        {
            InitializeComponent();

            sheetList = new ObservableCollection<SheetData>();
            TBlockData = new ObservableCollection<Element>(TblockList);
            ViewData = new ObservableCollection<View>(ViewList);

            sheetGrid.ItemsSource = sheetList;
            cmbTitleblock.ItemsSource = TBlockData;
            cmbViews.ItemsSource = ViewData;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            sheetList.Add(new SheetData());
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach(SheetData curRow in sheetList)
                {
                    if (sheetGrid.SelectedItem == curRow)
                        sheetList.Remove(curRow);
                }
            }
            catch (Exception)
            {}
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            sheetList.Clear();

            OpenFileDialog selectFile = new OpenFileDialog();
            selectFile.Multiselect = false;
            selectFile.RestoreDirectory = true;
            selectFile.Filter = "*csv file (*.csv)|*.csv";

            if (selectFile.ShowDialog() == true)
            {
                // read the csv file
                string[] sheetArray = System.IO.File.ReadAllLines(selectFile.FileName);

                foreach(string sheetString in sheetArray)
                {
                    string[] cellData = sheetString.Split(',');

                    SheetData curSD = new SheetData();
                    curSD.SheetNumber = cellData[0];
                    curSD.SheetName = cellData[1];

                    if (cellData[2] == "true")
                        curSD.IsPlaceholder = true;
                    else
                        curSD.IsPlaceholder = false;

                    // add method to get view by name

                    // add method to get titleblock by name

                    sheetList.Add(curSD);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {


        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; 
            this.Close();
        }

        public List<SheetData> GetSheetData()
        {
            return sheetList.ToList();
        }
    }
}
