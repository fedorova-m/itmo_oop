namespace DeliverySystem.Orders
{
    public class ExpressOrder : Order
    {
        private const decimal ExpressFee = 50m;

        public override decimal CalculateBasePrice()
        {
            decimal total = 0m;
            foreach (var item in Items)
            {
                total += item.GetTotalPrice();
            }
            return total + ExpressFee;
        }

        public override string GetOrderType()
        {
            return "Экспресс";
        }
    }
}

