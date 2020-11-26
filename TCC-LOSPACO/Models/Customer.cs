namespace TCC_LOSPACO.Models {
    public class Customer {
        public Account Account { get; private set; }
        public string Username { get; private set; }
        public string CPF { get; private set; }
        public string Phone { get; private set; }

        public Customer(Account account, string username, string cPF, string phone) {
            Account = account;
            Username = username;
            CPF = cPF;
            Phone = phone;
        }

        public Customer() { }

    }
}