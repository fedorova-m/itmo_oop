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
        orderManager.SubscribeObserver(new CustomerNotification());

        Console.WriteLine("=== Система управления заказами ===\n");

        var dish1 = new Dish(1, "Пицца Маргарита", 500, "Классическая пицца");
        var dish2 = new Dish(2, "Бургер", 350, "Сочный бургер");
        var dish3 = new Dish(3, "Салат Цезарь", 250, "Свежий салат");

        Console.WriteLine("--- 1. Abstract Factory Pattern ---");
        IOrderFactory factory = new OrderFactory();
        var order1 = factory.CreateStandardOrder(1, "Иван");
        order1.Items.Add(new OrderItem(dish1, 2));
        order1.Items.Add(new OrderItem(dish2, 1));
        orderManager.AddOrder(order1);

        var order2 = factory.CreateExpressOrder(2, "Мария");
        order2.Items.Add(new OrderItem(dish3, 3));
        orderManager.AddOrder(order2);

        var order3 = factory.CreateCustomOrder(3, "Алексей", "Без лука");
        order3.Items.Add(new OrderItem(dish1, 1));
        order3.Items.Add(new OrderItem(dish3, 2));
        orderManager.AddOrder(order3);
        Console.WriteLine();

        Console.WriteLine("--- 2. Builder Pattern ---");
        var order4 = new OrderBuilder()
            .SetId(4)
            .SetCustomerName("Елена")
            .SetOrderType("standard")
            .Build();
        order4.Items.Add(new OrderItem(dish2, 2));
        order4.Items.Add(new OrderItem(dish3, 1));
        orderManager.AddOrder(order4);
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
            Console.WriteLine($"Заказ {order.Id}: {order.GetOrderType()}, {order.CustomerName}, {order.State.GetStateName()}");
        }
    }
}
