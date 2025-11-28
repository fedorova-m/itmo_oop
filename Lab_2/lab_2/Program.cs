using InventorySystem.Inventory;
using InventorySystem.Items;
using InventorySystem.Patterns.Builder;
using InventorySystem.Patterns.Strategy;
using InventorySystem.Patterns.AbstractFactory;

class Program
{
    static void Main()
    {
        var inventory = new Inventory();

        Console.WriteLine("--- 1. Проверяем работу Abstract Factory Pattern ---");
        Console.WriteLine("Создание предметов через NoviceItemFactory:");
        IItemFactory noviceFactory = new NoviceItemFactory();
        var noviceWeapon = noviceFactory.CreateWeapon();
        var noviceArmor = noviceFactory.CreateArmor();
        var novicePotion = noviceFactory.CreatePotion();
        inventory.AddItem(noviceWeapon);
        inventory.AddItem(noviceArmor);
        inventory.AddItem(novicePotion);
        Console.WriteLine();

        Console.WriteLine("Создание предметов через MageItemFactory:");
        IItemFactory mageFactory = new MageItemFactory();
        var mageWeapon = mageFactory.CreateWeapon();
        var mageArmor = mageFactory.CreateArmor();
        var magePotion = mageFactory.CreatePotion();
        inventory.AddItem(mageWeapon);
        inventory.AddItem(mageArmor);
        inventory.AddItem(magePotion);
        Console.WriteLine();

        Console.WriteLine("--- 2. Проверяем работу Builder Pattern ---");
        Console.WriteLine("Создание оружия через WeaponBuilder:");
        var customSword = new WeaponBuilder()
            .SetName("Легендарный меч")
            .SetDamage(50)
            .Build();
        inventory.AddItem(customSword);
        Console.WriteLine();

        Console.WriteLine("--- 3. Различные типы предметов ---");
        var questItem = new QuestItem() 
        { 
            Name = "Древний артефакт", 
            Level = 1,
            QuestDescription = "Принесите этот артефакт в храм для завершения квеста"
        };
        inventory.AddItem(questItem);
        Console.WriteLine();

        Console.WriteLine("--- 4. Strategy Pattern ---");
        Console.WriteLine("Использование EquipStrategy:");
        inventory.UseItem("Простой меч", new EquipStrategy());
        inventory.UseItem("Легендарный меч", new EquipStrategy());
        Console.WriteLine();

        Console.WriteLine("Использование DrinkStrategy:");
        inventory.UseItem("Малое зелье", new DrinkStrategy());
        inventory.UseItem("Эликсир маны", new DrinkStrategy());
        Console.WriteLine();

        Console.WriteLine("Использование NoUseStrategy:");
        inventory.UseItem("Древний артефакт", new NoUseStrategy());
        Console.WriteLine();

        Console.WriteLine("--- 5. Улучшение предметов ---");
        Console.WriteLine("Улучшение оружия:");
        inventory.UpgradeItem("Простой меч");
        inventory.UpgradeItem("Простой меч");
        Console.WriteLine();

        Console.WriteLine("--- 6. State Pattern (состояния предметов) ---");
        Console.WriteLine("Использование предмета несколько раз для демонстрации изменения состояния:");
        inventory.UseItem("Простой меч", new EquipStrategy());
        inventory.UseItem("Простой меч", new EquipStrategy());
        Console.WriteLine("Третье использование (предмет сломается):");
        inventory.UseItem("Простой меч", new EquipStrategy());
        Console.WriteLine("Починка предмета через улучшение:");
        inventory.UpgradeItem("Простой меч");
        Console.WriteLine();

        Console.WriteLine("--- 7. Финальное состояние инвентаря ---");
        inventory.ShowItems();
        Console.WriteLine();
    }
}
