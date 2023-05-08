#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#endregion

namespace RAA_Level_02_Challenges
{
    [Transaction(TransactionMode.Manual)]
    public class cmdSheetMaker : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document; 
            
            // put any code needed for the form here

            FilteredElementCollector tblockCollector = new FilteredElementCollector(doc);
            tblockCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            tblockCollector.WhereElementIsElementType();

            // open form
            frmSheetMaker curForm = new frmSheetMaker(tblockCollector.ToList(), GetViews(doc))
            {
                Width = 600,
                Height = 450,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            curForm.ShowDialog();

            if(curForm.DialogResult == true)
            {
                int counter = 0;

                using(Transaction t = new Transaction(doc))
                {
                    t.Start("CReate new sheets");

                    // get form data and so something
                    foreach (SheetData curData in curForm.GetSheetData())
                    {

                        try
                        {
                            ViewSheet newSheet;

                            if (curData.IsPlaceholder == true)
                            {
                                newSheet = ViewSheet.CreatePlaceholder(doc);
                            }
                            else
                            {
                                newSheet = ViewSheet.Create(doc, curData.Titleblock.Id);
                            }

                            newSheet.SheetNumber = curData.SheetNumber;
                            newSheet.Name = curData.SheetName;
                        }
                        catch (Exception ex)
                        {
                            TaskDialog.Show("Error", "An error has occured: " + ex.Message);
                        }                        
                    }

                    t.Commit();
                }                
            }
         
            return Result.Succeeded;
        }

        private List<View> GetViews(Document doc)
        {
            List<View> m_returnList = new List<View>();

            FilteredElementCollector m_viewCollector = new FilteredElementCollector(doc);
            m_viewCollector.OfCategory(BuiltInCategory.OST_Views);

            FilteredElementCollector m_sheetCollector = new FilteredElementCollector(doc);
            m_sheetCollector.OfCategory(BuiltInCategory.OST_Sheets);

            foreach (View curView in m_viewCollector)
            {
                if (curView.IsTemplate == false)
                {
                    if (Viewport.CanAddViewToSheet(doc, m_sheetCollector.FirstElementId(), curView.Id) == true)
                        m_returnList.Add(curView);
                }
            }

            return m_returnList;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
