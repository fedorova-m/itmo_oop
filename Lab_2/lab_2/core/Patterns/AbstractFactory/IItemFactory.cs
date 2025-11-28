using InventorySystem.Items;

namespace InventorySystem.Patterns.AbstractFactory
{
    public interface IItemFactory
    {
        Weapon CreateWeapon();
        Armor CreateArmor();
        Potion CreatePotion();
    }
}
