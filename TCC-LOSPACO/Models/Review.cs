namespace TCC_LOSPACO.Models {
    public class Review {
        public decimal Rating { get; private set; }
        public string Comment { get; private set; }
        public string CostumerName { get; private set; }

        public Review(decimal rating, string comment, string costumerName) {
            Rating = rating;
            Comment = comment;
            CostumerName = costumerName;
        }

        public Review() { }
    }
}