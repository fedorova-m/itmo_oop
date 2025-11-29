namespace VendingMachine.Core
{
    public static class Calc
    {
        public static bool TryMakeChange(int amountKop, Wallet machineWallet, out Dictionary<int, int> change)
        {
            change = new Dictionary<int, int>();
            int remaining = amountKop;

            foreach (var denom in Money.SupportedCoins)
            {
                if (remaining <= 0)
                    break;

                int need = remaining / denom;
                int available = machineWallet.GetCount(denom);
                int take = need < available ? need : available;

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
