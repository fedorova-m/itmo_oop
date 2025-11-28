using InventorySystem.Items;

namespace InventorySystem.Patterns.State
{
    public class NewState : IItemState
    {
        public void Use(Item item)
        {
            Console.WriteLine($"Предмет {item.Name} используется (новый)");
            item.UseCount++;
            item.ChangeState(new UsedState());
        }

        public void Upgrade(Item item)
        {
            item.Level++;
            Console.WriteLine($"{item.Name} улучшен до уровня {item.Level} (новый)");
        }

        public string GetStateName()
        {
            return "Новый";
        }
    }
}

