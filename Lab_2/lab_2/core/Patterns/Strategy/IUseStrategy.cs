using InventorySystem.Items;

namespace InventorySystem.Patterns.Strategy
{
    public interface IUseStrategy
    {
        void Execute(Item item);
    }
}
