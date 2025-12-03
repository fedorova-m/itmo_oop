using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.Observer
{
    public class SMSNotification : IOrderObserver
    {
        public void Update(Order order, string message)
        {
            Console.WriteLine($"[SMS] Отправлено на телефон клиенту {order.CustomerName}: {message}");
        }
    }
}

