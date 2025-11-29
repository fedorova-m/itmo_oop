using DeliverySystem.Orders;
using DeliverySystem.Dishes;

namespace DeliverySystem.Patterns.AbstractFactory
{
    public interface IOrderFactory
    {
        Order CreateStandardOrder(int id, string customerName);
        Order CreateExpressOrder(int id, string customerName);
        Order CreateCustomOrder(int id, string customerName, string specialInstructions);
    }
}

