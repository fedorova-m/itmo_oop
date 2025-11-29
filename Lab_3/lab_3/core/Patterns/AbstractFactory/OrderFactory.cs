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

        public Order CreateCustomOrder(int id, string customerName, string specialInstructions)
        {
            var order = new CustomOrder();
            order.Id = id;
            order.CustomerName = customerName;
            order.SpecialInstructions = specialInstructions;
            return order;
        }
    }
}

