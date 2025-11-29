using DeliverySystem.Orders;
using DeliverySystem.Dishes;

namespace DeliverySystem.Patterns.Builder
{
    public class OrderBuilder
    {
        private int _orderId;
        private string? _customerName;
        private string? _orderType;

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

        public OrderBuilder SetOrderType(string type)
        {
            _orderType = type;
            return this;
        }

        public Order Build()
        {
            if (string.IsNullOrEmpty(_customerName))
            {
                throw new InvalidOperationException("Имя клиента не может быть пустым");
            }

            if (_orderId <= 0)
            {
                throw new InvalidOperationException("ID заказа должен быть положительным числом");
            }

            Order order;
            if (_orderType == "standard")
            {
                order = new StandardOrder();
            }
            else if (_orderType == "express")
            {
                order = new ExpressOrder();
            }
            else if (_orderType == "custom")
            {
                order = new CustomOrder();
            }
            else
            {
                throw new InvalidOperationException("Неизвестный тип заказа");
            }

            order.Id = _orderId;
            order.CustomerName = _customerName;

            return order;
        }
    }
}

