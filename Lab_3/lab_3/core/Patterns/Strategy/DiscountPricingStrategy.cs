using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.Strategy
{
    public class DiscountPricingStrategy : IPricingStrategy
    {
        private const decimal TaxRate = 0.10m;
        private const decimal DeliveryFee = 100m;
        private const decimal DiscountRate = 0.15m;

        public decimal CalculateFinalPrice(Order order)
        {
            decimal basePrice = order.CalculateBasePrice();
            decimal discount = basePrice * DiscountRate;
            decimal priceAfterDiscount = basePrice - discount;
            decimal tax = priceAfterDiscount * TaxRate;
            return priceAfterDiscount + tax + DeliveryFee;
        }
    }
}

