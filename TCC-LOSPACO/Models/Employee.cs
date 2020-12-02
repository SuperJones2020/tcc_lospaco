namespace TCC_LOSPACO.Models {
    public class Employee {
        public Customer Customer { get; private set; }
        public string Birth { get; set; }
        public string RG { get; private set; }
        public decimal Salary { get; private set; }
        public char Genre { get; private set; }
        public byte[] Image { get; private set; }

        public Employee(Customer customer, string birth, string rG, decimal salary, char genre, byte[] image) {
            Customer = customer;
            Birth = birth;
            RG = rG;
            Salary = salary;
            Genre = genre;
            Image = image;
        }

        public Employee() { }
    }
}