#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using Application = Autodesk.Revit.ApplicationServices.Application;
#endregion

namespace QApps
{
    [Transaction(TransactionMode.Manual)]
    public class TextNoteToDetailRebarCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            using (TransactionGroup transGroup = new TransactionGroup(doc, "TextNoteToDetailRebarCmd"))
            {
                transGroup.Start();
                while (true)
                {
                    try
                    {
                        Reference r1 = uidoc.Selection.PickObject(ObjectType.Element,
                            new TextNoteSelectionFilter(),
                            "Pick Text Notes to copy value");
                        TextNote textNote = doc.GetElement(r1.ElementId) as TextNote;
                        if (textNote == null) continue;

                        Reference r2 = uidoc.Selection.PickObject(ObjectType.Element,
                            new DetailRebarSelectionFilter(),
                            "Pick Text Notes to copy value");

                        Element detailRebar = doc.GetElement(r2.ElementId);
                        if (detailRebar == null) continue;
                        Parameter barDiameter = detailRebar.LookupParameter("D");
                        Parameter spacing = detailRebar.LookupParameter("S");
                        Parameter n = detailRebar.LookupParameter("n");


                        // textNote = T10-150 B2
                        // textNote = 2T10-300 T2
                        string textNoteValue = textNote.Text.Trim();
                        char[] separator = new[] 
                            {
                                Convert.ToChar("T"),
                                Convert.ToChar("-"),
                                Convert.ToChar(" "),
                            };
                        string[] allValues = textNoteValue.Split(separator);

                        // textNote = ["",10,150,B2]
                        // textNote = [2,10,300,"",2]

                        //MessageBox.Show(allValues.Length.ToString());
                        //MessageBox.Show(allValues[0] + "\n" + allValues[1] + "\n" +
                        //                allValues[2] + "\n" +allValues[3] + "\n"+
                        //                allValues[4] + "\n");
                        
                        using (Transaction trans = new Transaction(doc))
                        {
                            trans.Start("TextNoteToDetailRebarCmd");

                            // textNote = ["",10,150,B2] - T10-150 B2
                            if (string.IsNullOrEmpty(allValues[0]))
                            {
                                n.Set(Convert.ToInt16(allValues[1]));
                                barDiameter.Set(Convert.ToInt16(allValues[1]));
                                spacing.Set(Convert.ToInt16(allValues[2]));
                            }
                            else //textNote = [2, 10, 300, "", 2] - 2T10-300 T2
                            {
                                n.Set(Convert.ToInt16(allValues[0]));
                                barDiameter.Set(Convert.ToInt16(allValues[1]));
                                spacing.Set(Convert.ToInt16(allValues[2]));
                            }

                            trans.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
                transGroup.Commit();
            }

            return Result.Succeeded;
        }
    }
}
