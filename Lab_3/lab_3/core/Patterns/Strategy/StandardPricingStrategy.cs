using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.Strategy
{
    public class StandardPricingStrategy : IPricingStrategy
    {
        private const decimal TaxRate = 0.10m;
        private const decimal DeliveryFee = 100m;

        public decimal CalculateFinalPrice(Order order)
        {
            decimal basePrice = order.CalculateBasePrice();
            decimal tax = basePrice * TaxRate;
            return basePrice + tax + DeliveryFee;
        }
    }
}

