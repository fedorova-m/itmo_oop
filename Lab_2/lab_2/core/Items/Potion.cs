namespace InventorySystem.Items
{
    public class Potion : Item
    {
        public string? Effect { get; set; }

        public override void Use()
        {
            Console.WriteLine($"Вы использовали зелье: {Effect}");
            GetState().Use(this);
        }
    }
}
