using DeliverySystem.Orders;

namespace DeliverySystem.Patterns.State
{
    public class PreparingState : IOrderState
    {
        public void Process(Order order)
        {
            Console.WriteLine($"Заказ {order.Id} готовится");
        }

        public void Deliver(Order order)
        {
            Console.WriteLine($"Заказ {order.Id} переходит в доставку");
            order.ChangeState(new DeliveringState());
        }

        public void Complete(Order order)
        {
            Console.WriteLine("Нельзя завершить - заказ еще не доставлен");
        }

        public string GetStateName()
        {
            return "Подготовка";
        }
    }
}

