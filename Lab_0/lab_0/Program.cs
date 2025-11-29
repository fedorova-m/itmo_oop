using VM = VendingMachine.Core;

namespace Main
{
    class Program
    {
        static void Main()
        {
            var products = new List<VM.Product>();
            products.Add(new VM.Product(1, "Water", 200, 5));
            products.Add(new VM.Product(2, "Soda", 300, 5));
            products.Add(new VM.Product(3, "Juice", 300, 5));

            var machineCoins = new Dictionary<int, int>();
            machineCoins[100] = 10;
            machineCoins[50] = 10;
            machineCoins[20] = 10;
            machineCoins[10] = 10;
            var machineWallet = new VM.Wallet(machineCoins);

            var vm = new VM.VendingMachine(products, machineWallet, "admin123");

            Console.WriteLine("=== Торговый автомат ===");
            
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"Баланс: {VM.Money.Format(vm.CurrentBalanceKop)}");
                Console.WriteLine("1) Показать товары");
                Console.WriteLine("2) Вставить монету (1, 0.5, 0.2, 0.1 рубля)");
                Console.WriteLine("3) Купить товар");
                Console.WriteLine("4) Отмена / вернуть монеты");
                Console.WriteLine("5) Режим администратора");
                Console.WriteLine("0) Выход");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        ShowProducts(vm);
                        break;

                    case "2":
                        InsertCoin(vm);
                        break;

                    case "3":
                        BuyProduct(vm);
                        break;

                    case "4":
                        var result = vm.Cancel();
                        Console.WriteLine(result.message);
                        break;

                    case "5":
                        AdminMode(vm);
                        break;

                    case "0":
                        Console.WriteLine("До свидания!");
                        return;

                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }
        }

        static void ShowProducts(VM.VendingMachine vm)
        {
            Console.WriteLine("--- Товары ---");
            foreach (var product in vm.Products)
            {
                Console.WriteLine($"{product.Id}. {product.Name,-15} | Цена: {VM.Money.Format(product.PriceKop),8} | В наличии: {product.Quantity}");
            }
        }

        static void InsertCoin(VM.VendingMachine vm)
        {
            Console.Write("Введите монету (1 / 0.5 / 0.2 / 0.1): ");
            var input = Console.ReadLine()?.Trim().Replace(',', '.');
            
            if (input == null || !double.TryParse(input, out var rubles))
            {
                Console.WriteLine("Неверный ввод.");
                return;
            }

            var kopecks = (int)Math.Round(rubles * 100);
            
            if (!VM.Money.IsSupportedCoin(kopecks) || kopecks <= 0)
            {
                Console.WriteLine("Неподдерживаемая монета.");
                return;
            }

            vm.InsertCoin(kopecks);
            Console.WriteLine($"Вставлено {VM.Money.Format(kopecks)}.");
        }

        static void BuyProduct(VM.VendingMachine vm)
        {
            Console.Write("Введите ID товара: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            var result = vm.Purchase(id);
            Console.WriteLine(result.message);
        }

        static void AdminMode(VM.VendingMachine vm)
        {
            Console.Write("Введите пароль: ");
            var password = Console.ReadLine() ?? "";

            if (!vm.TryEnterAdmin(password))
            {
                Console.WriteLine("Доступ запрещен.");
                return;
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("--- Режим администратора ---");
                Console.WriteLine("1) Пополнить товар");
                Console.WriteLine("2) Загрузить монеты");
                Console.WriteLine("3) Собрать все деньги");
                Console.WriteLine("0) Выйти из режима администратора");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        Console.Write("ID товара: ");
                        if (!int.TryParse(Console.ReadLine(), out var productId))
                        {
                            Console.WriteLine("Неверный ID.");
                            break;
                        }
                        Console.Write("Количество для добавления: ");
                        if (!int.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
                        {
                            Console.WriteLine("Неверное количество.");
                            break;
                        }
                        Console.WriteLine(vm.AdminRestockProduct(productId, amount));
                        break;

                    case "2":
                        Console.Write("Монета (1 / 0.5 / 0.2 / 0.1): ");
                        var coinInput = Console.ReadLine()?.Trim().Replace(',', '.');
                        if (coinInput == null || !double.TryParse(coinInput, out var coinRubles))
                        {
                            Console.WriteLine("Неверный ввод.");
                            break;
                        }
                        var coinKopecks = (int)Math.Round(coinRubles * 100);
                        Console.Write("Количество монет: ");
                        if (!int.TryParse(Console.ReadLine(), out var coinCount) || coinCount <= 0)
                        {
                            Console.WriteLine("Неверное количество.");
                            break;
                        }
                        Console.WriteLine(vm.AdminLoadCoins(coinKopecks, coinCount));
                        break;

                    case "3":
                        Console.WriteLine(vm.AdminCollectAll());
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
        }
    }
}
