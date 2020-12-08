using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class CustomerDAO {
        private static Database db = new Database();
        public static List<Customer> GetList() {
            var list = new List<Customer>();
            db.ReaderRows(db.ReturnCommand("select * from tbCustomer"), row => list.Add(new Customer(AccountDAO.GetById((uint)row[0]), (string)row[1], (string)row[2], (string)row[3], row[4] + "")));
            return list;
        }

        public static Customer GetById(uint id) {
            string value = AccountDAO.RegistrationCompleted(id) ? "tbCustomer" : "tblogin";
            Customer customer = null;
            if (AccountDAO.RegistrationCompleted(id)) {
                var row = db.ReaderRow(db.ReturnCommand($"select * from tbcustomer where LoginId = '{id}'"));
                customer = new Customer(AccountDAO.GetById(id), (string)row[1], (string)row[2], (string)row[3], row[4] + "");
            } else {
                var row = db.ReaderRow(db.ReturnCommand($"select * from tblogin where LoginId = '{id}'"));
                customer = new Customer(AccountDAO.GetById(id), "", "", "", "");
            }
            return customer;
        }

        public static Customer GetByCPF(string cpf) {
            var row = db.ReaderRow(db.ReturnCommand($"select * from tbcustomer where custcpf = '{cpf}'"));
            return new Customer(AccountDAO.GetById((uint)row[0]), (string)row[1], (string)row[2], (string)row[3], row[4] + "");
        }

    }
}