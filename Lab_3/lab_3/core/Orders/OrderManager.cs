using DeliverySystem.Patterns.Strategy;
using DeliverySystem.Patterns.Observer;

namespace DeliverySystem.Orders
{
    public class OrderManager
    {
        private List<Order> _orders = new List<Order>();
        private OrderNotifier _notifier = new OrderNotifier();

        public void AddOrder(Order order)
        {
            _orders.Add(order);
            _notifier.Notify(order, "Заказ создан");
        }

        public Order? GetOrder(int id)
        {
            foreach (var order in _orders)
            {
                if (order.Id == id)
                {
                    return order;
                }
            }
            return null;
        }

        public List<Order> GetAllOrders()
        {
            return new List<Order>(_orders);
        }

        public void ProcessOrder(int id)
        {
            var order = GetOrder(id);
            if (order != null)
            {
                order.State.Process(order);
                _notifier.Notify(order, "Заказ в процессе подготовки");
            }
        }

        public void DeliverOrder(int id)
        {
            var order = GetOrder(id);
            if (order != null)
            {
                order.State.Deliver(order);
                _notifier.Notify(order, "Заказ отправлен на доставку");
            }
        }

        public void CompleteOrder(int id)
        {
            var order = GetOrder(id);
            if (order != null)
            {
                order.State.Complete(order);
                _notifier.Notify(order, "Заказ выполнен");
            }
        }

        public decimal CalculateOrderPrice(int id, IPricingStrategy strategy)
        {
            var order = GetOrder(id);
            if (order == null)
            {
                return 0;
            }
            return strategy.CalculateFinalPrice(order);
        }

        public void SubscribeObserver(IOrderObserver observer)
        {
            _notifier.Attach(observer);
        }
    }
}

