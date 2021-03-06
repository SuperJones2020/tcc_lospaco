﻿namespace TCC_LOSPACO.Models {
    public class Category {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public byte[] Image { get; private set; }

        public Category(uint id, string name, byte[] image) {
            Id = id;
            Name = name;
            Image = image;
        }

    }
}