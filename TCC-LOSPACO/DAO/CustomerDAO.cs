using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class CustomerDAO {
        public static IEnumerable<Customer> GetList() {
            var list = new List<Customer>();
            Database.ReaderRows(Database.ReturnCommand("select * from tbCustomer"), row => list.Add(new Customer((uint)row[0], AccountDAO.GetById((uint)row[0]), (string)row[1], (string)row[2], row[3] + "")));
            return list;
        }

        public static Customer GetById(uint id) {
            var row = Database.ReaderRow(Database.ReturnCommand($"select * from tbCustomer where LoginId = '{id}'"));
            if (row.Length == 0) return new Customer();
            Customer customer = new Customer((uint)row[0], AccountDAO.GetById(id), (string)row[1], (string)row[2], row[3] + "");
            return customer;
        }

    }
}