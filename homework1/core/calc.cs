namespace VendingMachine.Core
{
    public static class Calc
    {
        public static bool TryMakeChange(
            int amountKop,
            Wallet machineWallet,
            out Dictionary<int, int> change)
        {
            change = new Dictionary<int, int>();
            var remaining = amountKop;

            foreach (var denom in Money.SupportedCoins)
            {
                if (remaining <= 0) break;

                var need = remaining / denom;
                var avail = machineWallet.GetCount(denom);
                var take = System.Math.Min(need, avail);

                if (take > 0)
                {
                    change[denom] = take;
                    remaining -= take * denom;
                }
            }

            return remaining == 0;
        }
    }
}
