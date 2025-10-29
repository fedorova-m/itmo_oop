namespace VendingMachine.Core
{
    public sealed class Wallet
    {
        private readonly Dictionary<int, int> _coins = new();

        public Wallet()
        {
            foreach (var d in Money.SupportedCoins)
                _coins[d] = 0;
        }

        public Wallet(IDictionary<int, int> initial)
        {
            foreach (var d in Money.SupportedCoins)
                _coins[d] = initial.TryGetValue(d, out var c) ? c : 0;
        }

        public int GetCount(int denom) => _coins.TryGetValue(denom, out var c) ? c : 0;

        public void Add(int denom, int count = 1)
        {
            if (!_coins.ContainsKey(denom)) _coins[denom] = 0;
            _coins[denom] += count;
        }

        public bool TryRemove(int denom, int count = 1)
        {
            if (GetCount(denom) < count) return false;
            _coins[denom] -= count;
            return true;
        }

        public int TotalKop()
        {
            var sum = 0;
            foreach (var kv in _coins)
                sum += kv.Key * kv.Value;
            return sum;
        }

        public IReadOnlyDictionary<int, int> Snapshot() => _coins;
    }
}
