namespace DeliverySystem.Orders
{
    public class ScheduledOrder : Order
    {
        public string? ScheduledDeliveryTime { get; set; }
        private const decimal SchedulingFee = 25m;

        public override decimal CalculateBasePrice()
        {
            decimal total = 0m;
            foreach (var item in Items)
            {
                total += item.GetTotalPrice();
            }
            return total + SchedulingFee;
        }

        public override string GetOrderType()
        {
            return "К определенному времени";
        }
    }
}

