﻿namespace TCC_LOSPACO.Models {
    public class Account {
        public uint Id { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public Role Role { get; private set; }

        public Account(uint id, string email, string password, Role role) {
            Id = id;
            Email = email;
            Password = password;
            Role = Role;
        }

        /* public Account(string email, string password) {
             Email = email;
             Password = password;
         }*/
    }
}