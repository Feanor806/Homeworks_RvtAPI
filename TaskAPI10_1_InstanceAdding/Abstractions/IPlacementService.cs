using CSharpFunctionalExtensions;
using TaskAPI10_1_InstanceAdding.Models;

namespace TaskAPI10_1_InstanceAdding.Abstractions
{
    public interface IPlacementService
    {
        Result Place(FurnitureType selectedFurnitureType, int count);
    }
}
