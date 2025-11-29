using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.Observer
{
    public class OrderNotifier
    {
        private List<IOrderObserver> _observers = new List<IOrderObserver>();

        public void Attach(IOrderObserver observer)
        {
            _observers.Add(observer);
        }

        public void Notify(Order order, string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(order, message);
            }
        }
    }
}

