namespace InventorySystem.Items
{
    public class QuestItem : Item
    {
        public string? QuestDescription { get; set; }

        public override void Use()
        {
            if (!string.IsNullOrEmpty(QuestDescription))
            {
                Console.WriteLine($"Квестовый предмет {Name}: {QuestDescription}");
                Console.WriteLine("Этот предмет нельзя использовать напрямую. Он нужен для выполнения квеста.");
            }
            else
            {
                Console.WriteLine($"Квестовый предмет {Name} нельзя использовать. Он нужен для выполнения квеста.");
            }
        }
    }
}
