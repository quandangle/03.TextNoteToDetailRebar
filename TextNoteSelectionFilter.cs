#region Namespaces

using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
#endregion

namespace QApps
{
    public class TextNoteSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem.Category.Name.Equals("Text Notes");
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
