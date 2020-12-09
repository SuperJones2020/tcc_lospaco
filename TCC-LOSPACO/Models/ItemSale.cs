namespace TCC_LOSPACO.Models {
    public class ItemSale {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public ItemSale(uint id, string name, decimal price) {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}