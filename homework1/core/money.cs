using System.Globalization;

namespace VendingMachine.Core
{
    public static class Money
    {
        public const int Ruble = 100;

        public static readonly int[] SupportedCoins = new[] { 100, 50, 20, 10 };

        public static string Format(int amountKop)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:0.00} RUB", amountKop / 100.0);
        }

        public static bool IsSupportedCoin(int kop) => Array.Exists(SupportedCoins, c => c == kop);
    }
}
