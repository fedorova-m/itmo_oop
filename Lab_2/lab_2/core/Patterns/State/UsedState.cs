using InventorySystem.Items;

namespace InventorySystem.Patterns.State
{
    public class UsedState : IItemState
    {
        public void Use(Item item)
        {
            Console.WriteLine($"Предмет {item.Name} используется (уже был в использовании)");
            item.UseCount++;
            if (item.UseCount >= 3)
            {
                Console.WriteLine($"Предмет {item.Name} сломался от износа после {item.UseCount} использований!");
                item.ChangeState(new BrokenState());
            }
        }

        public void Upgrade(Item item)
        {
            item.Level++;
            Console.WriteLine($"{item.Name} улучшен до уровня {item.Level} (использованный)");
        }

        public string GetStateName()
        {
            return "Использованный";
        }
    }
}

