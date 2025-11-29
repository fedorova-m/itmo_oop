namespace DeliverySystem.Orders
{
    public class StandardOrder : Order
    {
        public override decimal CalculateBasePrice()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.GetTotalPrice();
            }
            return total;
        }

        public override string GetOrderType()
        {
            return "Стандартный";
        }
    }
}

