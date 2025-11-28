using InventorySystem.Items;

namespace InventorySystem.Patterns.AbstractFactory
{
    public class NoviceItemFactory : IItemFactory
    {
        public Weapon CreateWeapon()
        {
            return new Weapon() { Name = "Простой меч", Damage = 5 };
        }

        public Armor CreateArmor()
        {
            return new Armor() { Name = "Кожаная броня", Defense = 2 };
        }

        public Potion CreatePotion()
        {
            return new Potion() { Name = "Малое зелье", Effect = "+20 HP" };
        }
    }
}
