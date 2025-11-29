using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.Strategy
{
    public interface IPricingStrategy
    {
        decimal CalculateFinalPrice(Order order);
    }
}

