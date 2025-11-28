using InventorySystem.Items;

namespace InventorySystem.Patterns.State
{
    public class BrokenState : IItemState
    {
        public void Use(Item item)
        {
            Console.WriteLine($"Предмет {item.Name} сломан и не может быть использован!");
        }

        public void Upgrade(Item item)
        {
            Console.WriteLine($"Предмет {item.Name} починен и восстановлен! Счетчик использований сброшен.");
            item.UseCount = 0;
            item.ChangeState(new NewState());
        }

        public string GetStateName()
        {
            return "Сломанный";
        }
    }
}

