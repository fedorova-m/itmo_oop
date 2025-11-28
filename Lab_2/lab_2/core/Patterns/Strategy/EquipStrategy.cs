using InventorySystem.Items;

namespace InventorySystem.Patterns.Strategy
{
    public class EquipStrategy : IUseStrategy
    {
        public void Execute(Item item)
        {
            if (item is Weapon || item is Armor)
            {
                item.Use();
                Console.WriteLine($"Предмет {item.Name} экипирован");
            }
            else
            {
                Console.WriteLine($"Предмет {item.Name} нельзя экипировать");
            }
        }
    }
}
