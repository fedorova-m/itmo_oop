using InventorySystem.Items;

namespace InventorySystem.Patterns.Strategy
{
    public class DrinkStrategy : IUseStrategy
    {
        public void Execute(Item item)
        {
            if (item is Potion potion)
            {
                item.Use();
                Console.WriteLine($"Зелье {item.Name} использовано, эффект применен");
            }
            else
            {
                Console.WriteLine($"Предмет {item.Name} не является зельем");
            }
        }
    }
}
