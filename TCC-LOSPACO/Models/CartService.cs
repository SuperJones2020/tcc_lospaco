namespace TCC_LOSPACO.Models {
    public class CartService {
        public ushort Id { get; private set; }
        public string Name { get; private set; }
        public byte Quantity { get; private set; }
        public decimal Price { get; private set; }
        public byte[] Image { get; private set; }

        public CartService(ushort id, string name, byte quantity, decimal price, byte[] image) {
            Id = id;
            Name = name;
            Quantity = quantity;
            Price = price;
            Image = image;
        }

        public CartService() { }
    }
}