namespace InventorySystem.Items
{
    public class Armor : Item
    {
        public int Defense { get; set; }

        public override void Use()
        {
            Console.WriteLine($"Вы экипировали броню {Name} с защитой {Defense}");
            GetState().Use(this);
        }
    }
}
