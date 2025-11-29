using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.Strategy
{
    public class NoDeliveryFeeStrategy : IPricingStrategy
    {
        private const decimal TaxRate = 0.10m;

        public decimal CalculateFinalPrice(Order order)
        {
            decimal basePrice = order.CalculateBasePrice();
            decimal tax = basePrice * TaxRate;
            return basePrice + tax;
        }
    }
}

