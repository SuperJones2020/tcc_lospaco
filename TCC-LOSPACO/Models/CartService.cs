namespace TCC_LOSPACO.Models {
    public class CartService {
        public string Name { get; private set; }
        public byte Quantity { get; private set; }
        public decimal Price { get; private set; }
        public byte[] Image { get; private set; }

        public CartService(string name, byte quantity, decimal price, byte[] image) {
            Name = name;
            Quantity = quantity;
            Price = price;
            Image = image;
        }

        public CartService() { }
    }
}