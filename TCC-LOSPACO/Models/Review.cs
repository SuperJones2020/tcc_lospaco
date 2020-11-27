namespace TCC_LOSPACO.Models {
    public class Review {
        public decimal Rating { get; private set; }
        public string Comment { get; private set; }
        public Customer Customer { get; private set; }

        public Review(decimal rating, string comment, Customer customer) {
            Rating = rating;
            Comment = comment;
            Customer = customer;
        }

        public Review() { }
    }
}