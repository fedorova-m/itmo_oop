namespace InventorySystem.Items
{
    public class Weapon : Item
    {
        public int Damage { get; set; }

        public override void Use()
        {
            Console.WriteLine($"Вы экипировали оружие {Name} с уроном {Damage}");
            GetState().Use(this);
        }
    }
}
