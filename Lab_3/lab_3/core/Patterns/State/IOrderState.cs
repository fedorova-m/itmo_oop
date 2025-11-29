using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.State
{
    public interface IOrderState
    {
        void Process(Order order);
        void Deliver(Order order);
        void Complete(Order order);
        string GetStateName();
    }
}

