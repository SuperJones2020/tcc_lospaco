namespace TCC_LOSPACO.Models {
    public class Role {
        public byte Id { get; private set; }
        public string Name { get; private set; }

        public Role(byte id, string name) {
            Id = id;
            Name = name;
        }
    }
}