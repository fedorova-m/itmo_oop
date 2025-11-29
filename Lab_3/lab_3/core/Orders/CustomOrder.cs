namespace DeliverySystem.Orders
{
    public class CustomOrder : Order
    {
        public string? SpecialInstructions { get; set; }
        private const decimal CustomFee = 30m;

        public override decimal CalculateBasePrice()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.GetTotalPrice();
            }
            return total + CustomFee;
        }

        public override string GetOrderType()
        {
            return "Персональный";
        }
    }
}

