using Xunit;
using DeliverySystem.Orders;
using DeliverySystem.Dishes;
using DeliverySystem.Patterns.State;
using DeliverySystem.Patterns.Strategy;
using DeliverySystem.Patterns.Builder;
using DeliverySystem.Patterns.AbstractFactory;

namespace DeliverySystem.Tests
{
    public class OrderTests
    {
        [Fact]
        public void StandardOrder_ShouldCalculateBasePrice()
        {
            var order = new StandardOrder();
            order.Items.Add(new OrderItem(new Dish(1, "Пицца", 500, "Описание"), 2));
            order.Items.Add(new OrderItem(new Dish(2, "Бургер", 350, "Описание"), 1));

            decimal price = order.CalculateBasePrice();
            Assert.Equal(1350m, price);
        }

        [Fact]
        public void ExpressOrder_ShouldAddExpressFee()
        {
            var order = new ExpressOrder();
            order.Items.Add(new OrderItem(new Dish(1, "Пицца", 500, "Описание"), 1));

            decimal price = order.CalculateBasePrice();
            Assert.Equal(550m, price);
        }

        [Fact]
        public void CustomOrder_ShouldAddCustomFee()
        {
            var order = new CustomOrder();
            order.Items.Add(new OrderItem(new Dish(1, "Пицца", 500, "Описание"), 1));

            decimal price = order.CalculateBasePrice();
            Assert.Equal(530m, price);
        }
    }

    public class StateTests
    {
        [Fact]
        public void Order_ShouldStartInPreparingState()
        {
            var order = new StandardOrder();
            Assert.Equal("Подготовка", order.GetState().GetStateName());
        }

        [Fact]
        public void Order_ShouldTransitionToDeliveringState()
        {
            var order = new StandardOrder();
            order.GetState().Deliver(order);
            Assert.Equal("Доставка", order.GetState().GetStateName());
        }

        [Fact]
        public void Order_ShouldTransitionToCompletedState()
        {
            var order = new StandardOrder();
            order.GetState().Deliver(order);
            order.GetState().Complete(order);
            Assert.Equal("Выполнен", order.GetState().GetStateName());
        }
    }

    public class StrategyTests
    {
        [Fact]
        public void StandardPricingStrategy_ShouldCalculateWithTaxAndDelivery()
        {
            var order = new StandardOrder();
            order.Items.Add(new OrderItem(new Dish(1, "Пицца", 1000, "Описание"), 1));
            var strategy = new StandardPricingStrategy();

            decimal price = strategy.CalculateFinalPrice(order);
            Assert.Equal(1200m, price);
        }

        [Fact]
        public void DiscountPricingStrategy_ShouldApplyDiscount()
        {
            var order = new StandardOrder();
            order.Items.Add(new OrderItem(new Dish(1, "Пицца", 1000, "Описание"), 1));
            var strategy = new DiscountPricingStrategy();

            decimal price = strategy.CalculateFinalPrice(order);
            Assert.True(price < 1210m);
        }

        [Fact]
        public void NoDeliveryFeeStrategy_ShouldNotIncludeDeliveryFee()
        {
            var order = new StandardOrder();
            order.Items.Add(new OrderItem(new Dish(1, "Пицца", 1000, "Описание"), 1));
            var strategy = new NoDeliveryFeeStrategy();

            decimal price = strategy.CalculateFinalPrice(order);
            Assert.Equal(1100m, price);
        }
    }

    public class BuilderTests
    {
        [Fact]
        public void OrderBuilder_ShouldBuildStandardOrder()
        {
            var order = new OrderBuilder()
                .SetId(1)
                .SetCustomerName("Иван")
                .SetOrderType("standard")
                .Build();

            Assert.Equal(1, order.Id);
            Assert.Equal("Иван", order.CustomerName);
            Assert.Equal("Стандартный", order.GetOrderType());
        }

        [Fact]
        public void OrderBuilder_ShouldThrowOnEmptyName()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new OrderBuilder()
                    .SetId(1)
                    .SetCustomerName("")
                    .SetOrderType("standard")
                    .Build();
            });
        }
    }

    public class FactoryTests
    {
        [Fact]
        public void OrderFactory_ShouldCreateStandardOrder()
        {
            IOrderFactory factory = new OrderFactory();
            var order = factory.CreateStandardOrder(1, "Иван");

            Assert.Equal(1, order.Id);
            Assert.Equal("Иван", order.CustomerName);
            Assert.Equal("Стандартный", order.GetOrderType());
        }

        [Fact]
        public void OrderFactory_ShouldCreateExpressOrder()
        {
            IOrderFactory factory = new OrderFactory();
            var order = factory.CreateExpressOrder(2, "Мария");

            Assert.Equal("Экспресс", order.GetOrderType());
        }

        [Fact]
        public void OrderFactory_ShouldCreateCustomOrder()
        {
            IOrderFactory factory = new OrderFactory();
            var order = factory.CreateCustomOrder(3, "Алексей", "Без лука");

            Assert.Equal("Персональный", order.GetOrderType());
            if (order is CustomOrder customOrder)
            {
                Assert.Equal("Без лука", customOrder.SpecialInstructions);
            }
        }
    }

    public class OrderManagerTests
    {
        [Fact]
        public void OrderManager_ShouldAddOrder()
        {
            var manager = new OrderManager();
            var order = new StandardOrder();
            order.Id = 1;
            order.CustomerName = "Иван";

            manager.AddOrder(order);

            var retrieved = manager.GetOrder(1);
            Assert.NotNull(retrieved);
            Assert.Equal("Иван", retrieved.CustomerName);
        }

        [Fact]
        public void OrderManager_ShouldProcessOrder()
        {
            var manager = new OrderManager();
            var order = new StandardOrder();
            order.Id = 1;
            order.CustomerName = "Иван";
            manager.AddOrder(order);

            manager.ProcessOrder(1);

            var retrieved = manager.GetOrder(1);
            Assert.NotNull(retrieved);
        }
    }
}

