namespace VendingMachine.Core
{
    public class VendingMachine
    {
        private List<Product> products;
        private Wallet machineWallet;
        private Wallet sessionWallet;
        private string adminPassword;

        public VendingMachine(List<Product> products, Wallet machineWallet, string adminPassword)
        {
            this.products = products;
            this.machineWallet = machineWallet;
            this.sessionWallet = new Wallet();
            this.adminPassword = adminPassword;
        }

        public List<Product> Products => products;
        public int CurrentBalanceKop => sessionWallet.TotalKop();

        public bool InsertCoin(int denomKop)
        {
            if (!Money.IsSupportedCoin(denomKop))
                return false;

            sessionWallet.Add(denomKop);
            return true;
        }

        public (bool ok, string message) Purchase(int productId)
        {
            Product? product = null;
            foreach (var p in products)
            {
                if (p.Id == productId)
                {
                    product = p;
                    break;
                }
            }

            if (product == null)
                return (false, "Товар не найден");

            if (product.Quantity <= 0)
                return (false, "Товар закончился");

            if (CurrentBalanceKop < product.PriceKop)
                return (false, "Недостаточно средств");

            var sessionCoins = sessionWallet.Snapshot();
            foreach (var coin in sessionCoins)
            {
                if (coin.Value > 0)
                {
                    machineWallet.Add(coin.Key, coin.Value);
                }
            }

            var changeKop = CurrentBalanceKop - product.PriceKop;
            Dictionary<int, int> change = new Dictionary<int, int>();

            if (changeKop > 0)
            {
                if (!Calc.TryMakeChange(changeKop, machineWallet, out change))
                {
                    foreach (var coin in sessionCoins)
                    {
                        if (coin.Value > 0)
                        {
                            machineWallet.TryRemove(coin.Key, coin.Value);
                        }
                    }
                    return (false, "Невозможно выдать сдачу для этой операции");
                }

                foreach (var coin in change)
                {
                    machineWallet.TryRemove(coin.Key, coin.Value);
                }
            }

            product.ConsumeOne();

            ClearSession();

            if (changeKop == 0)
            {
                return (true, "Наслаждайтесь покупкой!");
            }
            else
            {
                var changeDescription = DescribeCoins(change);
                return (true, $"Наслаждайтесь покупкой! Сдача: {changeDescription} ({Money.Format(changeKop)})");
            }
        }

        public (bool ok, string message) Cancel()
        {
            var coins = sessionWallet.Snapshot();
            var sum = CurrentBalanceKop;
            ClearSession();

            if (coins.Count == 0)
            {
                return (true, "Нечего возвращать");
            }

            var coinsDescription = DescribeCoins(coins);
            return (true, $"Возвращено: {coinsDescription} ({Money.Format(sum)})");
        }

        public bool TryEnterAdmin(string password)
        {
            return password == adminPassword;
        }

        public string AdminCollectAll()
        {
            var total = machineWallet.TotalKop();
            foreach (var coin in Money.SupportedCoins)
            {
                var count = machineWallet.GetCount(coin);
                machineWallet.TryRemove(coin, count);
            }
            return $"Собрано {Money.Format(total)}";
        }

        public string AdminRestockProduct(int id, int amount)
        {
            Product? product = null;
            foreach (var p in products)
            {
                if (p.Id == id)
                {
                    product = p;
                    break;
                }
            }

            if (product == null)
                return "Товар не найден";

            product.AddStock(amount);
            return $"Товар '{product.Name}' теперь в количестве {product.Quantity}";
        }

        public string AdminLoadCoins(int denom, int count)
        {
            if (!Money.IsSupportedCoin(denom))
                return "Неподдерживаемая монета";

            machineWallet.Add(denom, count);
            return $"Загружено {count} монет(ы) номиналом {Money.Format(denom)}";
        }

        private void ClearSession()
        {
            foreach (var coin in Money.SupportedCoins)
            {
                var count = sessionWallet.GetCount(coin);
                if (count > 0)
                {
                    sessionWallet.TryRemove(coin, count);
                }
            }
        }

        private string DescribeCoins(Dictionary<int, int> coins)
        {
            var descriptions = new List<string>();
            foreach (var coin in coins)
            {
                if (coin.Value > 0)
                {
                    descriptions.Add($"{Money.Format(coin.Key)}x{coin.Value}");
                }
            }
            return string.Join(", ", descriptions);
        }
    }
}
