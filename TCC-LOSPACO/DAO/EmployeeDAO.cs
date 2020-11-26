using System;
using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public class EmployeeDAO {
        public static IEnumerable<Employee> GetList() {
            var list = new List<Employee>();
            string query = "select LoginId, EmpName, EmpBirthDate, EmpCPF, EmpRG, EmpRG_n, EmpGender, EmpPhoneNumber, EmpSalary, EmpImage from tbEmployee";
            Database.ReaderRows(Database.ReturnCommand(query), row => list.Add(new Employee(AccountDAO.GetById((uint)row[0]), (string)row[1], row[2].ToString(), (string)row[3], row[4].ToString(),
                Convert.ToChar(row[5]), Convert.ToChar(row[6]), row[7].ToString(), (decimal)row[8], row[9].ToString())));
            return list;
        }

        public static Employee GetByEmail(string email) {
            uint loginId = AccountDAO.GetByEmail(email).Id;
            var row = Database.ReaderRow(Database.ReturnCommand($"select LoginId, EmpName, EmpBirthDate, EmpCPF, EmpRG, EmpRG_n, EmpGender, EmpPhoneNumber, EmpSalary, EmpImage from tbEmployee where LoginId = '{loginId}'"));
            Employee employee = new Employee(AccountDAO.GetById((uint)row[0]), (string)row[1], row[2].ToString(), (string)row[3], row[4].ToString(),
                Convert.ToChar(row[5]), Convert.ToChar(row[6]), row[7].ToString(), (decimal)row[8], row[9].ToString());
            return employee;
        }
    }
}