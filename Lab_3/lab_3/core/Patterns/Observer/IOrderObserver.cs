using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.Observer
{
    public interface IOrderObserver
    {
        void Update(Order order, string message);
    }
}

