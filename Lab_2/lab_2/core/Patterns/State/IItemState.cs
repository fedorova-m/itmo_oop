using InventorySystem.Items;

namespace InventorySystem.Patterns.State
{
    public interface IItemState
    {
        void Use(Item item);
        void Upgrade(Item item);
        string GetStateName();
    }
}

