using Autodesk.Revit.DB;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskAPI10_1_InstanceAdding.Abstractions;
using TaskAPI10_1_InstanceAdding.Models;

namespace TaskAPI10_1_InstanceAdding.Services
{
    public class PlacementService : IPlacementService
    {
        private readonly Document _document;

        public PlacementService(Document document)
        {
            _document = document;
        }

        public Result Place(FurnitureType furnitureType, int count)
        {
            return Validate(count)
                .Bind(() => FindFamily(furnitureType))
                .Bind(s => PlaceInstance(s, count));
        }

        private Result Validate(int count)
        {
            if (count < 0)
                return Result.Failure("Количество должно быть больше 0");
            return Result.Success();
        }

        private Result<FamilySymbol> FindFamily(FurnitureType furnitureType)
        {
            string furnitureName = string.Empty;
            switch (furnitureType)
            {
                case FurnitureType.Table:
                    furnitureName = "Стол";
                    break;
                case FurnitureType.Chair:
                    furnitureName = "Стул";
                    break;
                case FurnitureType.Cabinet:
                    furnitureName = "Шкаф";
                    break;
            }

            FamilySymbol familySymbol = new FilteredElementCollector(_document)
                .OfCategory(BuiltInCategory.OST_Furniture)
                .OfClass(typeof(FamilySymbol))
                .OfType<FamilySymbol>()
                .Where(x => x.FamilyName.Contains(furnitureName))
                .FirstOrDefault();

            if (familySymbol == null)
                return Result.Failure<FamilySymbol>("Не найден типоразмер для размещения.");
            return familySymbol;
        }

        private Result PlaceInstance(FamilySymbol familySymbol, int count)
        {
            try
            {
                double step = UnitUtils.ConvertToInternalUnits(2, DisplayUnitType.DUT_METERS);
                var points = new List<XYZ>();
                int rowMax = (int)Math.Ceiling(Math.Sqrt(count));
                for(int i = 0; i < rowMax; i++)
                {
                    for (int j = 0; (j < rowMax) && (i*rowMax + j < count); j++)
                    {
                        points.Add(new XYZ(j * step, i * step, 0));
                    }
                }

                var level = new FilteredElementCollector(_document)
                    .OfClass(typeof(Level))
                    .OfType<Level>()
                    .OrderBy(l => l.Elevation)
                    .FirstOrDefault();

                if (level == null)
                    return Result.Failure("Не удалось определить уровень для размещения");

                using (Transaction transaction = new Transaction(_document, "Размещение мебели"))
                {
                    transaction.Start();

                    if (!familySymbol.IsActive)
                    {
                        familySymbol.Activate();
                    }

                    foreach (var point in points)
                    {
                        _document.Create.NewFamilyInstance(
                            point,
                            familySymbol,
                            level,
                            Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
    
                }

                    transaction.Commit();
                }
                return Result.Success();
            }
            catch(Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
