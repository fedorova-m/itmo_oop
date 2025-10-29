using VendingMachine.Core;
using Microsoft.Extensions.Configuration;

namespace Main
{
    internal static class Program
    {
        private static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("products.json", optional: false, reloadOnChange: true)
                .Build();

            var adminPassword = configuration["AdminPassword"] ?? "oop";

            var products = new List<Product>();
            var productsSection = configuration.GetSection("Products");
            foreach (var productSection in productsSection.GetChildren())
            {
                var id = int.Parse(productSection["Id"] ?? "0");
                var name = productSection["Name"] ?? "";
                var priceKop = int.Parse(productSection["PriceKop"] ?? "0");
                var quantity = int.Parse(productSection["Quantity"] ?? "0");
                products.Add(new Product(id, name, priceKop, quantity));
            }

            var machineWallet = new Wallet(new Dictionary<int, int>
            {
                [100] = 10,
                [50] = 10,
                [20] = 10,
                [10] = 10
            });

            var vm = new VendingMachine.Core.VendingMachine(products, machineWallet, adminPassword: adminPassword);

            Console.WriteLine("== Vending Machine ==");
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"Balance: {Money.Format(vm.CurrentBalanceKop)}");
                Console.WriteLine("1) Show products");
                Console.WriteLine("2) Insert coin (1, 0.5, 0.2, 0.1 RUB)");
                Console.WriteLine("3) Buy product");
                Console.WriteLine("4) Cancel / return coins");
                Console.WriteLine("5) Admin mode");
                Console.WriteLine("0) Exit");
                Console.Write("Choose: ");

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
                        Purchase(vm);
                        break;

                    case "4":
                        var cancel = vm.Cancel();
                        Console.WriteLine(cancel.message);
                        break;

                    case "5":
                        Admin(vm);
                        break;

                    case "0":
                        Console.WriteLine("Bye");
                        return;

                    default:
                        Console.WriteLine("Unknown option");
                        break;
                }
            }
        }

        private static void ShowProducts(VendingMachine.Core.VendingMachine vm)
        {
            Console.WriteLine("-- Products --");
            foreach (var p in vm.Products)
            {
                Console.WriteLine($"{p.Id}. {p.Name,-15} | Price: {Money.Format(p.PriceKop),8} | Stock: {p.Quantity}");
            }
        }

        private static void InsertCoin(VendingMachine.Core.VendingMachine vm)
        {
            Console.Write("Enter coin (1 / 0.5 / 0.2 / 0.1): ");
            var s = Console.ReadLine()?.Trim().Replace(',', '.');
            if (!double.TryParse(s, System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.InvariantCulture, out var rub))
            {
                Console.WriteLine("Invalid input.");
                return;
            }
            var kop = (int)Math.Round(rub * 100);
            if (!Money.IsSupportedCoin(kop) || kop <= 0)
            {
                Console.WriteLine("Unsupported coin.");
                return;
            }
            vm.InsertCoin(kop);
            Console.WriteLine($"Inserted {Money.Format(kop)}.");
        }

        private static void Purchase(VendingMachine.Core.VendingMachine vm)
        {
            Console.Write("Enter product id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id.");
                return;
            }
            var result = vm.Purchase(id);
            Console.WriteLine(result.message);
        }

        private static void Admin(VendingMachine.Core.VendingMachine vm)
        {
            Console.Write("Password: ");
            var pwd = Console.ReadLine();
            if (!vm.TryEnterAdmin(pwd ?? string.Empty))
            {
                Console.WriteLine("Access denied.");
                return;
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("-- Admin --");
                Console.WriteLine("1) Restock product");
                Console.WriteLine("2) Load coins");
                Console.WriteLine("3) Collect all money");
                Console.WriteLine("0) Exit admin");
                Console.Write("Choose: ");
                var c = Console.ReadLine();
                Console.Clear();

                switch (c)
                {
                    case "1":
                        Console.Write("Product id: ");
                        if (!int.TryParse(Console.ReadLine(), out var id))
                        {
                            Console.WriteLine("Invalid.");
                            break;
                        }
                        Console.Write("Amount to add: ");
                        if (!int.TryParse(Console.ReadLine(), out var amt) || amt <= 0)
                        {
                            Console.WriteLine("Invalid.");
                            break;
                        }
                        Console.WriteLine(vm.AdminRestockProduct(id, amt));
                        break;

                    case "2":
                        Console.Write("Coin (1 / 0.5 / 0.2 / 0.1): ");
                        var s = Console.ReadLine()?.Trim().Replace(',', '.');
                        if (!double.TryParse(s, System.Globalization.NumberStyles.Any,
                                             System.Globalization.CultureInfo.InvariantCulture, out var rub))
                        {
                            Console.WriteLine("Invalid.");
                            break;
                        }
                        var denom = (int)Math.Round(rub * 100);
                        Console.Write("Count: ");
                        if (!int.TryParse(Console.ReadLine(), out var cnt) || cnt <= 0)
                        {
                            Console.WriteLine("Invalid.");
                            break;
                        }
                        Console.WriteLine(vm.AdminLoadCoins(denom, cnt));
                        break;

                    case "3":
                        Console.WriteLine(vm.AdminCollectAll());
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }
            }
        }
    }
}
