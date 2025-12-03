using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.AbstractFactory
{
    public class OrderFactory : IOrderFactory
    {
        public Order CreateStandardOrder(int id, string customerName)
        {
            var order = new StandardOrder();
            order.Id = id;
            order.CustomerName = customerName;
            return order;
        }

        public Order CreateExpressOrder(int id, string customerName)
        {
            var order = new ExpressOrder();
            order.Id = id;
            order.CustomerName = customerName;
            return order;
        }

        public Order CreateScheduledOrder(int id, string customerName, string scheduledTime)
        {
            var order = new ScheduledOrder();
            order.Id = id;
            order.CustomerName = customerName;
            order.ScheduledDeliveryTime = scheduledTime;
            return order;
        }
    }
}

