using InventorySystem.Items;

namespace InventorySystem.Patterns.AbstractFactory
{
    public class MageItemFactory : IItemFactory
    {
        public Weapon CreateWeapon()
        {
            return new Weapon() { Name = "Посох мага", Damage = 12 };
        }

        public Armor CreateArmor()
        {
            return new Armor() { Name = "Мантия мага", Defense = 3 };
        }

        public Potion CreatePotion()
        {
            return new Potion() { Name = "Эликсир маны", Effect = "+30 MP" };
        }
    }
}
