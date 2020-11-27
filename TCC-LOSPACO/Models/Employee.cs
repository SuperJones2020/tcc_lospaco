namespace TCC_LOSPACO.Models {
    public class Employee {
        public uint Id { get; private set; }
        public Account Account { get; private set; }
        public string Name { get; private set; }
        public string Birth { get; set; }
        public string CPF { get; private set; }
        public string RG { get; private set; }
        public char RGDig { get; private set; }
        public char Genre { get; private set; }
        public string Phone { get; private set; }
        public decimal Salary { get; private set; }
        public string Image { get; private set; }

        public Employee(uint id, Account account, string name, string birth, string cPF, string rG, char rGDig, char genre, string phone, decimal salary, string image) {
            Id = id;
            Account = account;
            Name = name;
            Birth = birth;
            CPF = cPF;
            RG = rG;
            RGDig = rGDig;
            Genre = genre;
            Phone = phone;
            Salary = salary;
            Image = image;
        }

        public Employee() { }
    }
}