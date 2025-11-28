using InventorySystem.Patterns.State;

namespace InventorySystem.Items
{
    public abstract class Item
    {
        public string? Name { get; set; }
        public int Level { get; set; } = 1;
        public int UseCount { get; set; } = 0;
        private IItemState _state;

        protected Item()
        {
            _state = new NewState();
        }

        public IItemState GetState()
        {
            return _state;
        }

        public void ChangeState(IItemState newState)
        {
            _state = newState;
        }

        public abstract void Use();

        public void Upgrade()
        {
            _state.Upgrade(this);
        }

        public string GetStateName()
        {
            return _state.GetStateName();
        }
    }
}
