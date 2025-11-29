namespace VendingMachine.Core
{
    public static class Money
    {
        public const int Ruble = 100;

        public static int[] SupportedCoins = new int[] { 100, 50, 20, 10 };

        public static string Format(int amountKop)
        {
            double rubles = amountKop / 100.0;
            return $"{rubles:F2} RUB";
        }

        public static bool IsSupportedCoin(int kop)
        {
            foreach (var coin in SupportedCoins)
            {
                if (coin == kop)
                    return true;
            }
            return false;
        }
    }
}
