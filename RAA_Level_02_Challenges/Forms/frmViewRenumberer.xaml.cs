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
using Autodesk.Revit.DB;


namespace RAA_Level_02_Challenges
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class frmViewRenumberer : Window
    {
        public List<Element> elemList;

        public frmViewRenumberer(Document doc, List<Reference> refList)
        {
            InitializeComponent();

            for(int i = 1; i <= 20; i++)
            {
                cmbNumber.Items.Add(i.ToString());
            }

            if(refList != null)
            {
                lbxElements.Items.Clear();
                elemList = new List<Element>();

                foreach(Reference curRef in refList)
                {
                    Element curElem = doc.GetElement(curRef);

                    if(curElem is Viewport)
                    {
                        elemList.Add(curElem);
                        lbxElements.Items.Add(curElem.ToString());
                    }
                }

            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
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
    }
}
