using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAA_Level_02_Challenges
{
    internal class SheetData
    {
        public string SheetNumber {  get; set; }
        public string SheetName { get; set;}
        public bool IsPlaceholder { get; set; }
        public Element Titleblock { get; set; }
    }
}
