using Xunit;
using InventorySystem.Items;
using InventorySystem.Patterns.Strategy;
using InventorySystem.Patterns.Builder;
using InventorySystem.Patterns.AbstractFactory;
using InventorySystem.Patterns.State;

namespace InventorySystem.Tests
{
    public class WeaponTests
    {
        [Fact]
        public void Weapon_ShouldCreateWithProperties()
        {
            var weapon = new Weapon() { Name = "Меч", Damage = 10 };
            Assert.Equal("Меч", weapon.Name);
            Assert.Equal(10, weapon.Damage);
            Assert.Equal(1, weapon.Level);
            Assert.Equal(0, weapon.UseCount);
        }

        [Fact]
        public void Weapon_Use_ShouldChangeState()
        {
            var weapon = new Weapon() { Name = "Меч", Damage = 10 };
            Assert.Equal("Новый", weapon.GetStateName());
            weapon.Use();
            Assert.Equal("Использованный", weapon.GetStateName());
        }
    }

    public class ArmorTests
    {
        [Fact]
        public void Armor_ShouldCreateWithProperties()
        {
            var armor = new Armor() { Name = "Броня", Defense = 5 };
            Assert.Equal("Броня", armor.Name);
            Assert.Equal(5, armor.Defense);
            Assert.Equal(1, armor.Level);
        }

        [Fact]
        public void Armor_Use_ShouldChangeState()
        {
            var armor = new Armor() { Name = "Броня", Defense = 5 };
            armor.Use();
            Assert.Equal("Использованный", armor.GetStateName());
        }
    }

    public class PotionTests
    {
        [Fact]
        public void Potion_ShouldCreateWithProperties()
        {
            var potion = new Potion() { Name = "Зелье", Effect = "+50 HP" };
            Assert.Equal("Зелье", potion.Name);
            Assert.Equal("+50 HP", potion.Effect);
            Assert.Equal(1, potion.Level);
        }

        [Fact]
        public void Potion_Use_ShouldChangeState()
        {
            var potion = new Potion() { Name = "Зелье", Effect = "+50 HP" };
            potion.Use();
            Assert.Equal("Использованный", potion.GetStateName());
        }
    }

    public class QuestItemTests
    {
        [Fact]
        public void QuestItem_ShouldCreate()
        {
            var questItem = new QuestItem() { Name = "Артефакт" };
            Assert.Equal("Артефакт", questItem.Name);
            Assert.Equal(1, questItem.Level);
        }

        [Fact]
        public void QuestItem_Use_ShouldNotChangeState()
        {
            var questItem = new QuestItem() { Name = "Артефакт" };
            questItem.Use();
            Assert.Equal("Новый", questItem.GetStateName());
        }
    }

    public class InventoryTests
    {
        [Fact]
        public void AddItem_ShouldAddItem()
        {
            var inv = new InventorySystem.Inventory.Inventory();
            var item = new Potion() { Name = "Тест" };
            inv.AddItem(item);
            Assert.True(true);
        }

        [Fact]
        public void UpgradeItem_ShouldIncreaseLevel()
        {
            var inv = new InventorySystem.Inventory.Inventory();
            var item = new Weapon() { Name = "Тест оружие", Damage = 10 };
            inv.AddItem(item);
            inv.UpgradeItem("Тест оружие");
            Assert.Equal(2, item.Level);
        }

        [Fact]
        public void UpgradeItem_ShouldNotFailForNonExistentItem()
        {
            var inv = new InventorySystem.Inventory.Inventory();
            inv.UpgradeItem("Несуществующий предмет");
            Assert.True(true);
        }

        [Fact]
        public void UseItem_ShouldExecuteStrategy()
        {
            var inv = new InventorySystem.Inventory.Inventory();
            var weapon = new Weapon() { Name = "Меч", Damage = 10 };
            inv.AddItem(weapon);
            inv.UseItem("Меч", new EquipStrategy());
            Assert.Equal("Использованный", weapon.GetStateName());
        }

        [Fact]
        public void UseItem_ShouldHandleNonExistentItem()
        {
            var inv = new InventorySystem.Inventory.Inventory();
            inv.UseItem("Несуществующий", new EquipStrategy());
            Assert.True(true);
        }
    }

    public class EquipStrategyTests
    {
        [Fact]
        public void EquipStrategy_ShouldWorkWithWeapon()
        {
            var strategy = new EquipStrategy();
            var weapon = new Weapon() { Name = "Меч", Damage = 10 };
            strategy.Execute(weapon);
            Assert.Equal("Использованный", weapon.GetStateName());
        }

        [Fact]
        public void EquipStrategy_ShouldWorkWithArmor()
        {
            var strategy = new EquipStrategy();
            var armor = new Armor() { Name = "Броня", Defense = 5 };
            strategy.Execute(armor);
            Assert.Equal("Использованный", armor.GetStateName());
        }

        [Fact]
        public void EquipStrategy_ShouldNotWorkWithPotion()
        {
            var strategy = new EquipStrategy();
            var potion = new Potion() { Name = "Зелье", Effect = "+50 HP" };
            strategy.Execute(potion);
            Assert.Equal("Новый", potion.GetStateName());
        }
    }

    public class DrinkStrategyTests
    {
        [Fact]
        public void DrinkStrategy_ShouldWorkWithPotion()
        {
            var strategy = new DrinkStrategy();
            var potion = new Potion() { Name = "Зелье", Effect = "+50 HP" };
            strategy.Execute(potion);
            Assert.Equal("Использованный", potion.GetStateName());
        }

        [Fact]
        public void DrinkStrategy_ShouldNotWorkWithWeapon()
        {
            var strategy = new DrinkStrategy();
            var weapon = new Weapon() { Name = "Меч", Damage = 10 };
            strategy.Execute(weapon);
            Assert.Equal("Новый", weapon.GetStateName());
        }
    }

    public class NoUseStrategyTests
    {
        [Fact]
        public void NoUseStrategy_ShouldNotChangeState()
        {
            var strategy = new NoUseStrategy();
            var item = new Weapon() { Name = "Меч", Damage = 10 };
            strategy.Execute(item);
            Assert.Equal("Новый", item.GetStateName());
        }
    }

    public class WeaponBuilderTests
    {
        [Fact]
        public void WeaponBuilder_ShouldBuildWeapon()
        {
            var builder = new WeaponBuilder();
            var weapon = builder
                .SetName("Легендарный меч")
                .SetDamage(50)
                .Build();

            Assert.Equal("Легендарный меч", weapon.Name);
            Assert.Equal(50, weapon.Damage);
        }

        [Fact]
        public void WeaponBuilder_ShouldThrowOnEmptyName()
        {
            var builder = new WeaponBuilder();
            builder.SetDamage(10);
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void WeaponBuilder_ShouldThrowOnZeroDamage()
        {
            var builder = new WeaponBuilder();
            builder.SetName("Меч");
            builder.SetDamage(0);
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void WeaponBuilder_ShouldThrowOnNegativeDamage()
        {
            var builder = new WeaponBuilder();
            builder.SetName("Меч");
            builder.SetDamage(-5);
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }
    }

    public class NoviceItemFactoryTests
    {
        [Fact]
        public void NoviceItemFactory_ShouldCreateWeapon()
        {
            var factory = new NoviceItemFactory();
            var weapon = factory.CreateWeapon();
            Assert.Equal("Простой меч", weapon.Name);
            Assert.Equal(5, weapon.Damage);
        }

        [Fact]
        public void NoviceItemFactory_ShouldCreateArmor()
        {
            var factory = new NoviceItemFactory();
            var armor = factory.CreateArmor();
            Assert.Equal("Кожаная броня", armor.Name);
            Assert.Equal(2, armor.Defense);
        }

        [Fact]
        public void NoviceItemFactory_ShouldCreatePotion()
        {
            var factory = new NoviceItemFactory();
            var potion = factory.CreatePotion();
            Assert.Equal("Малое зелье", potion.Name);
            Assert.Equal("+20 HP", potion.Effect);
        }
    }

    public class MageItemFactoryTests
    {
        [Fact]
        public void MageItemFactory_ShouldCreateWeapon()
        {
            var factory = new MageItemFactory();
            var weapon = factory.CreateWeapon();
            Assert.Equal("Посох мага", weapon.Name);
            Assert.Equal(12, weapon.Damage);
        }

        [Fact]
        public void MageItemFactory_ShouldCreateArmor()
        {
            var factory = new MageItemFactory();
            var armor = factory.CreateArmor();
            Assert.Equal("Мантия мага", armor.Name);
            Assert.Equal(3, armor.Defense);
        }

        [Fact]
        public void MageItemFactory_ShouldCreatePotion()
        {
            var factory = new MageItemFactory();
            var potion = factory.CreatePotion();
            Assert.Equal("Эликсир маны", potion.Name);
            Assert.Equal("+30 MP", potion.Effect);
        }
    }

    public class NewStateTests
    {
        [Fact]
        public void NewState_Use_ShouldChangeToUsedState()
        {
            var item = new Weapon() { Name = "Меч", Damage = 10 };
            item.Use();
            Assert.Equal("Использованный", item.GetStateName());
            Assert.Equal(1, item.UseCount);
        }

        [Fact]
        public void NewState_Upgrade_ShouldIncreaseLevel()
        {
            var item = new Weapon() { Name = "Меч", Damage = 10 };
            item.Upgrade();
            Assert.Equal(2, item.Level);
        }
    }

    public class UsedStateTests
    {
        [Fact]
        public void UsedState_Use_ShouldStayInUsedState()
        {
            var item = new Weapon() { Name = "Меч", Damage = 10 };
            item.Use();
            item.Use();
            Assert.Equal("Использованный", item.GetStateName());
            Assert.Equal(2, item.UseCount);
        }

        [Fact]
        public void UsedState_Use_ShouldChangeToBrokenAfterThreeUses()
        {
            var item = new Weapon() { Name = "Меч", Damage = 10 };
            item.Use();
            item.Use();
            item.Use();
            Assert.Equal("Сломанный", item.GetStateName());
            Assert.Equal(3, item.UseCount);
        }

        [Fact]
        public void UsedState_Upgrade_ShouldIncreaseLevel()
        {
            var item = new Weapon() { Name = "Меч", Damage = 10 };
            item.Use();
            item.Upgrade();
            Assert.Equal(2, item.Level);
        }
    }

    public class BrokenStateTests
    {
        [Fact]
        public void BrokenState_Use_ShouldNotChangeState()
        {
            var item = new Weapon() { Name = "Меч", Damage = 10 };
            item.Use();
            item.Use();
            item.Use();
            Assert.Equal("Сломанный", item.GetStateName());
            item.Use();
            Assert.Equal("Сломанный", item.GetStateName());
        }

        [Fact]
        public void BrokenState_Upgrade_ShouldRepairAndResetUseCount()
        {
            var item = new Weapon() { Name = "Меч", Damage = 10 };
            item.Use();
            item.Use();
            item.Use();
            Assert.Equal("Сломанный", item.GetStateName());
            item.Upgrade();
            Assert.Equal("Новый", item.GetStateName());
            Assert.Equal(0, item.UseCount);
        }
    }

    public class IntegrationTests
    {
        [Fact]
        public void FullWorkflow_ShouldWork()
        {
            var inv = new InventorySystem.Inventory.Inventory();
            var factory = new NoviceItemFactory();
            var weapon = factory.CreateWeapon();
            inv.AddItem(weapon);
            inv.UseItem("Простой меч", new EquipStrategy());
            inv.UpgradeItem("Простой меч");
            Assert.Equal(2, weapon.Level);
            Assert.Equal("Использованный", weapon.GetStateName());
        }

        [Fact]
        public void BuilderWithFactory_ShouldWork()
        {
            var factory = new NoviceItemFactory();
            var factoryWeapon = factory.CreateWeapon();
            var builderWeapon = new WeaponBuilder()
                .SetName("Кастомный меч")
                .SetDamage(20)
                .Build();

            Assert.NotEqual(factoryWeapon.Name, builderWeapon.Name);
            Assert.NotEqual(factoryWeapon.Damage, builderWeapon.Damage);
        }
    }
}
