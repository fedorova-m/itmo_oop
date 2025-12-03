using DeliverySystem.Patterns.State;
using DeliverySystem.Dishes;

namespace DeliverySystem.Orders
{
    public abstract class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public List<OrderItem> Items { get; set; }
        private IOrderState _state;

        protected Order()
        {
            Items = new List<OrderItem>();
            _state = new PreparingState();
        }

        public IOrderState GetState()
        {
            return _state;
        }

        public void ChangeState(IOrderState newState)
        {
            _state = newState;
        }

        public abstract decimal CalculateBasePrice();
        public abstract string GetOrderType();
    }

    public class OrderItem
    {
        public Dish Dish { get; set; }
        public int Quantity { get; set; }

        public OrderItem(Dish dish, int quantity)
        {
            Dish = dish;
            Quantity = quantity;
        }

        public decimal GetTotalPrice()
        {
            return Dish.Price * Quantity;
        }
    }
}

