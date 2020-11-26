using System.Collections.Generic;

namespace TCC_LOSPACO.Models {
    public class Cart {
        public IEnumerable<string> Names { get; private set; }
        public IEnumerable<byte> Quantities { get; private set; }
        public IEnumerable<string> Types { get; private set; }

        public Cart(IEnumerable<string> names, IEnumerable<byte> quantities, IEnumerable<string> types) {
            Names = names;
            Quantities = quantities;
            Types = types;
        }

        public Cart() { }
    }
}