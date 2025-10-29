namespace VendingMachine.Core
{
    public sealed class Product
    {
        public int Id { get; }
        public string Name { get; }
        public int PriceKop { get; }
        public int Quantity { get; private set; }

        public Product(int id, string name, int priceKop, int quantity)
        {
            Id = id;
            Name = name;
            PriceKop = priceKop;
            Quantity = quantity;
        }

        public bool HasStock => Quantity > 0;

        public void AddStock(int amount) => Quantity += amount;
        public void ConsumeOne() => Quantity--;
    }
}
