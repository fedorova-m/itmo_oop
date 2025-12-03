using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.Observer
{
    public class EmailNotification : IOrderObserver
    {
        public void Update(Order order, string message)
        {
            Console.WriteLine($"[Email] Письмо отправлено на почту клиенту {order.CustomerName}: {message}");
        }
    }
}

