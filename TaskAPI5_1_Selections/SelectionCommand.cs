using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI5_1_Selections
{
    [Transaction(TransactionMode.Manual)]
    public class SelectionCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            List<FamilyInstance> instances = new List<FamilyInstance>();

            try
            {
                IList<Reference> pickedRefs = uiDoc.Selection.PickObjects(ObjectType.Element, new FamilyInstanceFilter(), "Выберите элементы");
                foreach (var pickedRef in pickedRefs)
                {
                    Element element = doc.GetElement(pickedRef);
                    if (element is FamilyInstance)
                    {
                        instances.Add(element as FamilyInstance);
                    }
                }
            }
            catch
            {
                TaskDialog.Show("Ошибка", $"Элементы не выбраны");
                return Result.Failed;
            }
            if (instances.Count < 1) 
            {
                TaskDialog.Show("Ошибка", $"Не выбраны загружаемые семейства");
                return Result.Failed;
            }

            Dictionary<string,int> categoriesCount = new Dictionary<string,int>();
            foreach(FamilyInstance instance in instances)
            {
                Category category = instance.Category;
                if (!categoriesCount.ContainsKey(category.Name))
                {
                    categoriesCount[category.Name] = 1;
                }
                else
                {
                    categoriesCount[category.Name]++;
                }
            }

            string catsReport = String.Empty;

            foreach (string catName in categoriesCount.Keys)
            {
                catsReport += $"{catName}: {categoriesCount[catName]} шт.\n";
            }

            TaskDialog.Show("Инфо", $"Всего найдено элементов типа FamilyInstance: {instances.Count}.\nИз них:\n{catsReport}");
            return Result.Failed;
        }
    }

    internal class FamilyInstanceFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is FamilyInstance;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
