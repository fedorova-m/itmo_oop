namespace VendingMachine.Core
{
    public sealed class VendingMachine
    {
        private readonly List<Product> _products;
        private readonly Wallet _machineWallet;
        private readonly Wallet _sessionWallet;
        private readonly string _adminPassword;

        public VendingMachine(List<Product> products, Wallet machineWallet, string adminPassword)
        {
            _products = products;
            _machineWallet = machineWallet;
            _sessionWallet = new Wallet();
            _adminPassword = adminPassword;
        }

        public IReadOnlyList<Product> Products => _products;
        public int CurrentBalanceKop => _sessionWallet.TotalKop();

        public bool InsertCoin(int denomKop)
        {
            if (!Money.IsSupportedCoin(denomKop)) return false;
            _sessionWallet.Add(denomKop);
            return true;
        }

        public (bool ok, string message) Purchase(int productId)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product == null) return (false, "Product not found");
            if (!product.HasStock) return (false, "Out of stock");
            if (CurrentBalanceKop < product.PriceKop) return (false, "Insufficient balance");

            foreach (var kv in _sessionWallet.Snapshot())
                _machineWallet.Add(kv.Key, kv.Value);

            var changeKop = CurrentBalanceKop - product.PriceKop;
            if (!Calc.TryMakeChange(changeKop, _machineWallet, out var change))
            {
                foreach (var kv in _sessionWallet.Snapshot())
                    _machineWallet.TryRemove(kv.Key, kv.Value);
                return (false, "Cannot make change for this transaction");
            }

            product.ConsumeOne();

            foreach (var kv in change)
                _machineWallet.TryRemove(kv.Key, kv.Value);

            ClearSession();

            var msg = changeKop == 0 ? "Enjoy your product!" :
                      $"Enjoy your product! Change: {DescribeCoins(change)} ({Money.Format(changeKop)})";
            return (true, msg);
        }

        public (bool ok, string message) Cancel()
        {
            var coins = _sessionWallet.Snapshot().Where(kv => kv.Value > 0)
                .ToDictionary(k => k.Key, v => v.Value);
            var sum = CurrentBalanceKop;
            ClearSession();
            var msg = coins.Count == 0 ? "Nothing to return" :
                      $"Returned: {DescribeCoins(coins)} ({Money.Format(sum)})";
            return (true, msg);
        }

        public bool TryEnterAdmin(string password) => password == _adminPassword;

        public string AdminCollectAll()
        {
            var total = _machineWallet.TotalKop();
            foreach (var d in Money.SupportedCoins)
                _machineWallet.TryRemove(d, _machineWallet.GetCount(d));
            return $"Collected {Money.Format(total)}";
        }

        public string AdminRestockProduct(int id, int amount)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return "Product not found";
            product.AddStock(amount);
            return $"Product '{product.Name}' stock is now {product.Quantity}";
        }

        public string AdminLoadCoins(int denom, int count)
        {
            if (!Money.IsSupportedCoin(denom)) return "Unsupported coin";
            _machineWallet.Add(denom, count);
            return $"Loaded {count} coin(s) of {Money.Format(denom)}";
        }

        private void ClearSession()
        {
            foreach (var d in Money.SupportedCoins)
            {
                var c = _sessionWallet.GetCount(d);
                if (c > 0) _sessionWallet.TryRemove(d, c);
            }
        }

        public static string DescribeCoins(IDictionary<int, int> coins)
        {
            return string.Join(", ",
                coins.Where(kv => kv.Value > 0)
                     .OrderByDescending(kv => kv.Key)
                     .Select(kv => $"{Money.Format(kv.Key)}x{kv.Value}"));
        }
    }
}
