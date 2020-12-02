using System;
using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public class EmployeeDAO {
        private static Database db = new Database();
        public static List<Employee> GetList() {
            var list = new List<Employee>();
            string query = "select * from tbEmployee";
            db.ReaderRows(db.ReturnCommand(query), row => list.Add(new Employee(CustomerDAO.GetById((uint)row[0]), row[1] + "", row[2] + "", (decimal)row[3], Convert.ToChar(row[4]), (byte[])row[5])));
            return list;
        }

        public static Employee GetById(uint id) {
            var row = db.ReaderRow(db.ReturnCommand($"select * from tbEmployee where LoginId = '{id}'"));
            return new Employee(CustomerDAO.GetById((uint)row[0]), row[1] + "", (string)row[2], (decimal)row[3], Convert.ToChar(row[4]), (byte[])row[5]);
        }
    }
}