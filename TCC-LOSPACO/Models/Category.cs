namespace TCC_LOSPACO.Models {
    public class Category {
        public uint Id { get; private set; }
        public string Name { get; private set; }

        public Category(uint id, string name) {
            Id = id;
            Name = name;
        }

    }
}