using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.Observer
{
    public class CustomerNotification : IOrderObserver
    {
        public void Update(Order order, string message)
        {
            Console.WriteLine($"Уведомление клиенту {order.CustomerName}: {message}");
        }
    }
}

