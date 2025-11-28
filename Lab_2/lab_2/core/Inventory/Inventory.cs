using InventorySystem.Items;
using InventorySystem.Patterns.Strategy;

namespace InventorySystem.Inventory
{
    public class Inventory
    {
        private List<Item> items = new List<Item>();

        public void AddItem(Item item)
        {
            items.Add(item);
            Console.WriteLine($"{item.Name} добавлен в инвентарь");
        }

        public void ShowItems()
        {
            Console.WriteLine("--- Инвентарь ---");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name}, уровень {item.Level}, состояние: {item.GetStateName()}");
            }
        }

        public void UseItem(string name, IUseStrategy strategy)
        {
            Item? item = null;
            foreach (var i in items)
            {
                if (i.Name == name)
                {
                    item = i;
                    break;
                }
            }
            if (item != null)
                strategy.Execute(item);
            else
                Console.WriteLine("Предмет не найден");
        }

        public void UpgradeItem(string name)
        {
            Item? item = null;
            foreach (var i in items)
            {
                if (i.Name == name)
                {
                    item = i;
                    break;
                }
            }
            item?.Upgrade();
        }
    }
}

