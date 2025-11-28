using InventorySystem.Items;

namespace InventorySystem.Patterns.Builder
{
    public class WeaponBuilder
    {
        private Weapon weapon = new Weapon();

        public WeaponBuilder SetName(string name)
        {
            weapon.Name = name;
            return this;
        }

        public WeaponBuilder SetDamage(int dmg)
        {
            weapon.Damage = dmg;
            return this;
        }

        public Weapon Build()
        {
            if (string.IsNullOrEmpty(weapon.Name))
            {
                throw new InvalidOperationException("Имя оружия не может быть пустым");
            }
            if (weapon.Damage <= 0)
            {
                throw new InvalidOperationException("Урон оружия должен быть положительным числом");
            }
            return weapon;
        }
    }
}
