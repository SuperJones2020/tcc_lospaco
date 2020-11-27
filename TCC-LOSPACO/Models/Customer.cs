namespace TCC_LOSPACO.Models {
    public class Customer {
        public uint Id { get; private set; }
        public Account Account { get; private set; }
        public string Username { get; private set; }
        public string CPF { get; private set; }
        public string Phone { get; private set; }

        public Customer(uint id, Account account, string username, string cPF, string phone) {
            Id = id;
            Account = account;
            Username = username;
            CPF = cPF;
            Phone = phone;
        }

        public Customer() { }

    }
}