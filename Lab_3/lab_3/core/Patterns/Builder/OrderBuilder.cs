using DeliverySystem.Orders;
using DeliverySystem.Dishes;

namespace DeliverySystem.Patterns.Builder
{
    public class OrderBuilder
    {
        private int _orderId;
        private string? _customerName;
        private string? _specialInstructions;

        public OrderBuilder SetId(int id)
        {
            _orderId = id;
            return this;
        }

        public OrderBuilder SetCustomerName(string name)
        {
            _customerName = name;
            return this;
        }

        public OrderBuilder SetSpecialInstructions(string instructions)
        {
            _specialInstructions = instructions;
            return this;
        }

        public Order? Build()
        {
            if (string.IsNullOrEmpty(_customerName))
            {
                Console.WriteLine("Ошибка: Имя клиента не может быть пустым");
                return null;
            }

            if (_orderId <= 0)
            {
                Console.WriteLine("Ошибка: ID заказа должен быть положительным числом");
                return null;
            }

                var customOrder = new CustomOrder();
            customOrder.Id = _orderId;
            customOrder.CustomerName = _customerName;
                customOrder.SpecialInstructions = _specialInstructions;

            return customOrder;
        }
    }
}

