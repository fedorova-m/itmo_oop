namespace VendingMachine.Core
{
    public class Wallet
    {
        private Dictionary<int, int> coins;

        public Wallet()
        {
            coins = new Dictionary<int, int>();
            foreach (var coin in Money.SupportedCoins)
            {
                coins[coin] = 0;
            }
        }

        public Wallet(Dictionary<int, int> initial)
        {
            coins = new Dictionary<int, int>();
            foreach (var coin in Money.SupportedCoins)
            {
                if (initial.ContainsKey(coin))
                {
                    coins[coin] = initial[coin];
                }
                else
                {
                    coins[coin] = 0;
                }
            }
        }

        public int GetCount(int denom)
        {
            if (coins.ContainsKey(denom))
                return coins[denom];
            return 0;
        }

        public void Add(int denom, int count = 1)
        {
            if (!coins.ContainsKey(denom))
            {
                coins[denom] = 0;
            }
            coins[denom] += count;
        }

        public bool TryRemove(int denom, int count = 1)
        {
            if (GetCount(denom) < count)
                return false;

            coins[denom] -= count;
            return true;
        }

        public int TotalKop()
        {
            int sum = 0;
            foreach (var coin in coins)
            {
                sum += coin.Key * coin.Value;
            }
            return sum;
        }

        public Dictionary<int, int> Snapshot()
        {
            return new Dictionary<int, int>(coins);
        }
    }
}
