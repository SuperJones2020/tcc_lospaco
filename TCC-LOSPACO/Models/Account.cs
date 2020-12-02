namespace TCC_LOSPACO.Models {
    public class Account {
        public uint Id { get; private set; }
        public string Email { get; private set; }
        public Role Role { get; private set; }

        public Account(uint id, string email, Role role) {
            Id = id;
            Email = email;
            Role = Role;
        }

        /* public Account(string email, string password) {
             Email = email;
             Password = password;
         }*/
    }
}