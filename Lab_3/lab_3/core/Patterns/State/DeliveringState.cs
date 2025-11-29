using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.State
{
    public class DeliveringState : IOrderState
    {
        public void Process(Order order)
        {
            Console.WriteLine("Заказ уже в доставке");
        }

        public void Deliver(Order order)
        {
            Console.WriteLine("Заказ уже доставляется");
        }

        public void Complete(Order order)
        {
            Console.WriteLine($"Заказ {order.Id} выполнен");
            order.ChangeState(new CompletedState());
        }

        public string GetStateName()
        {
            return "Доставка";
        }
    }
}

