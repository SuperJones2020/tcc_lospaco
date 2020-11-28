using System;

namespace TCC_LOSPACO.Models {
    public class Service {
        public ushort Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string MinifiedDesc { get; private set; }
        public string CompleteDesc { get; private set; }
        public Category Category { get; private set; }
        public TimeSpan Time { get; private set; }
        public byte[] Image { get; private set; }
        public string PropperClothing { get; private set; }
        public decimal StarRating { get; private set; }

        public Service(ushort id, string name, decimal price, string minifiedDesc, string completeDesc, Category category, TimeSpan time, byte[] image, string propperClothing, decimal starRating) {
            Id = id;
            Name = name;
            Price = price;
            MinifiedDesc = minifiedDesc;
            CompleteDesc = completeDesc;
            Category = category;
            Time = time;
            Image = image;
            PropperClothing = propperClothing;
            StarRating = starRating;
        }

        public Service() { }
    }
}