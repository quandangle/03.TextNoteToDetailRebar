#region Namespaces

using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
#endregion

namespace QApps
{
    public class DetailRebarSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element el)
        {
            return el.Category.Name.Equals("Detail Items")
                   && el is FamilyInstance
                   && (el as FamilyInstance).Name.Equals("THONG KE COT THEP SAN_2020");
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
