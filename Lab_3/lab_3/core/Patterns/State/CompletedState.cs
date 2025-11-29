using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.State
{
    public class CompletedState : IOrderState
    {
        public void Process(Order order)
        {
            Console.WriteLine("Заказ уже выполнен");
        }

        public void Deliver(Order order)
        {
            Console.WriteLine("Заказ уже выполнен");
        }

        public void Complete(Order order)
        {
            Console.WriteLine("Заказ уже выполнен");
        }

        public string GetStateName()
        {
            return "Выполнен";
        }
    }
}

