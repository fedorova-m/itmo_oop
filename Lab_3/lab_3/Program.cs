using DeliverySystem.Orders;
using DeliverySystem.Dishes;
using DeliverySystem.Patterns.Builder;
using DeliverySystem.Patterns.Strategy;
using DeliverySystem.Patterns.AbstractFactory;
using DeliverySystem.Patterns.Observer;

class Program
{
    static void Main()
    {
        var orderManager = new OrderManager();

        Console.WriteLine("-- Система управления заказами --\n");

        var dish1 = new Dish(1, "Пицца Маргарита", 500, "Классическая пицца");
        var dish2 = new Dish(2, "Бургер", 350, "Сочный бургер");
        var dish3 = new Dish(3, "Салат Цезарь", 250, "Свежий салат");

        Console.WriteLine("--- 1. Abstract Factory Pattern (обычные заказы) ---");
        IOrderFactory factory = new OrderFactory();
        var order1 = factory.CreateStandardOrder(1, "Иван");
        order1.Items.Add(new OrderItem(dish1, 2));
        order1.Items.Add(new OrderItem(dish2, 1));
        orderManager.AddOrder(order1);

        var order2 = factory.CreateExpressOrder(2, "Мария");
        order2.Items.Add(new OrderItem(dish3, 3));
        orderManager.AddOrder(order2);

        var order4 = factory.CreateScheduledOrder(4, "Петр", "18:30");
        order4.Items.Add(new OrderItem(dish1, 1));
        order4.Items.Add(new OrderItem(dish2, 2));
        orderManager.AddOrder(order4);
        Console.WriteLine();

        Console.WriteLine("--- 2. Builder Pattern (персональный заказ) ---");
        var order3 = new OrderBuilder()
            .SetId(3)
            .SetCustomerName("Алексей")
            .SetSpecialInstructions("В бургер не добавлять лук. Двойная порция сыра в пиццу. Салат без сухариков.")
            .Build();
        if (order3 != null)
        {
            order3.Items.Add(new OrderItem(dish2, 1));
            order3.Items.Add(new OrderItem(dish1, 1));
            order3.Items.Add(new OrderItem(dish3, 1));
            orderManager.AddOrder(order3);
        }
        Console.WriteLine();

        Console.WriteLine("--- 3. State Pattern ---");
        orderManager.ProcessOrder(1);
        orderManager.DeliverOrder(1);
        orderManager.CompleteOrder(1);
        Console.WriteLine();

        Console.WriteLine("--- 4. Strategy Pattern ---");
        var standardStrategy = new StandardPricingStrategy();
        var discountStrategy = new DiscountPricingStrategy();
        var noDeliveryStrategy = new NoDeliveryFeeStrategy();

        Console.WriteLine($"Стандартная: {orderManager.CalculateOrderPrice(2, standardStrategy):F2} руб.");
        Console.WriteLine($"Со скидкой: {orderManager.CalculateOrderPrice(2, discountStrategy):F2} руб.");
        Console.WriteLine($"Без доставки: {orderManager.CalculateOrderPrice(2, noDeliveryStrategy):F2} руб.");
        Console.WriteLine();

        Console.WriteLine("--- 5. Observer Pattern ---");
        orderManager.ProcessOrder(3);
        orderManager.DeliverOrder(3);
        orderManager.CompleteOrder(3);
        Console.WriteLine();

        Console.WriteLine("--- Все заказы ---");
        foreach (var order in orderManager.GetAllOrders())
        {
            string orderInfo = $"Заказ {order.Id}: {order.GetOrderType()}, {order.CustomerName}, {order.GetState().GetStateName()}";
            if (order is ScheduledOrder scheduledOrder && !string.IsNullOrEmpty(scheduledOrder.ScheduledDeliveryTime))
            {
                orderInfo += $", доставка: {scheduledOrder.ScheduledDeliveryTime}";
            }
            Console.WriteLine(orderInfo);
        }
    }
}
