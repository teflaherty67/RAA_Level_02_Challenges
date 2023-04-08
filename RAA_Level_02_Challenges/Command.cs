#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace RAA_Level_02_Challenges
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
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

            if(doc.ActiveView is ViewSheet == false)
            {
                TaskDialog.Show("Error", "The current view is not a sheet");
                return Result.Failed;
            }

            // open form
            frmViewRenumberer curForm = new frmViewRenumberer(doc, null)
            {
                Width = 600,
                Height = 450,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            curForm.ShowDialog();

            if(curForm.DialogResult == false)
            {
                return Result.Failed;
            }

            List<Reference> refList = new List<Reference>();
            bool flag = true;

            while(flag == true)
            {
                try
                {
                    Reference curRef = uidoc.Selection.PickObject(ObjectType.Element, "Select views to renumber in order. Hit Esc when done.");
                    refList.Add(curRef);
                }
                catch (Exception)
                {
                    flag = false;
                }
            }

            // open the form again

            frmViewRenumberer curForm2 = new frmViewRenumberer(doc, refList)
            {
                Width = 600,
                Height = 450,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            curForm2.ShowDialog();

            if (curForm2.DialogResult == false)
            {
                return Result.Failed;
            }

            // set variables

            int counter = 0;
            int startNum = curForm2.GetStartNumber();
            int curNum = startNum;
            List<Element> viewList = curForm2.GetSelectedViews();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Renumber Views");

                foreach (Element curElem in viewList)
                {
                    Parameter curParam = curElem.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                    curParam.Set("z" + curNum.ToString());

                    curNum++;                   
                }

                curNum = startNum;

                foreach (Element curView in viewList)
                {
                    Parameter curParam = curView.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                    curParam.Set(curNum.ToString());

                    curNum++;
                    counter++;
                }

                t.Commit();
            }

            List<string> results = new List<string>();
            results.Add("Renumbered " + counter.ToString() + " views.");

            frmRenumberResults curResults = new frmRenumberResults(results)
            {
                Width = 500,
                Height = 350,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            curResults.ShowDialog();

            // TaskDialog.Show("Complete", "Renumbered " + counter.ToString() + " views.");

            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
