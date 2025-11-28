using InventorySystem.Items;

namespace InventorySystem.Patterns.Strategy
{
    public class NoUseStrategy : IUseStrategy
    {
        public void Execute(Item item)
        {
            Console.WriteLine("Этот предмет нельзя использовать");
        }
    }
}
