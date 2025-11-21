using VendingMachine.Core;
using System.Text.Json;

namespace Main
{
    internal static class Program
    {
        private static void Main()
        {
            string adminPassword = "oop";
            try
            {
                string appsettingsText = File.ReadAllText("appsettings.json");
                var appsettings = JsonSerializer.Deserialize<Dictionary<string, string>>(appsettingsText);
                if (appsettings != null && appsettings.ContainsKey("AdminPassword"))
                {
                    adminPassword = appsettings["AdminPassword"];
                }
            }
            catch
            {

            }

            var products = new List<Product>();
            try
            {
                string productsText = File.ReadAllText("products.json");
                var productsData = JsonSerializer.Deserialize<ProductsData>(productsText);
                if (productsData != null && productsData.Products != null)
                {
                    foreach (var productData in productsData.Products)
                    {
                        Product product = new Product(
                            productData.Id,
                            productData.Name,
                            productData.PriceKop,
                            productData.Quantity
                        );
                        products.Add(product);
                    }
                }
            }
            catch
            {
                products.Add(new Product(1, "Water", 200, 5));
                products.Add(new Product(2, "Soda", 300, 5));
                products.Add(new Product(3, "Juice", 300, 5));
            }

            var initialCoins = new Dictionary<int, int>();
            initialCoins[100] = 10;
            initialCoins[50] = 10;
            initialCoins[20] = 10;
            initialCoins[10] = 10;
            var machineWallet = new Wallet(initialCoins);

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
            if (s == null || !double.TryParse(s, out var rub))
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
            if (pwd == null)
            {
                pwd = "";
            }
            if (!vm.TryEnterAdmin(pwd))
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
                        if (s == null || !double.TryParse(s, out var rub))
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

    internal class ProductsData
    {
        public List<ProductData>? Products { get; set; }
    }

    internal class ProductData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int PriceKop { get; set; }
        public int Quantity { get; set; }
    }

    internal class AppSettings
    {
        public string? AdminPassword { get; set; }
    }
}
