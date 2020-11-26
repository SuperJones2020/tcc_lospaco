using System.Collections.Generic;

namespace TCC_LOSPACO.Models {
    public class Package {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public byte[] Image { get; private set; }
        public decimal Price { get; private set; }
        public List<Service> Services { get; private set; }

        public Package(uint id, string name, string description, byte[] image, decimal price, List<Service> services) {
            Id = id;
            Name = name;
            Description = description;
            Image = image;
            Price = price;
            Services = services;
        }

        public Package() { }
    }
}