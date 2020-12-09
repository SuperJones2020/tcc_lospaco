using System.Collections.Generic;

namespace TCC_LOSPACO.Models {
    public class Sale {
        public uint Id { get; private set; }
        public string Date { get; private set; }
        public Customer Customer { get; private set; }
        public List<ItemSale> Sales { get; private set; }

        public Sale(uint id, string date, Customer customer, List<ItemSale> sales) {
            Id = id;
            Date = date;
            Customer = customer;
            Sales = sales;
        }

    }
}