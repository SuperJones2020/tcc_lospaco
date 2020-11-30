using System;
using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public class EmployeeDAO {
        private static Database db = new Database();
        public static List<Employee> GetList() {
            var list = new List<Employee>();
            string query = "select LoginId, EmpName, EmpBirthDate, EmpCPF, EmpRG, EmpRG_n, EmpGender, EmpPhoneNumber, EmpSalary, EmpImage from tbEmployee";
            db.ReaderRows(db.ReturnCommand(query), row => list.Add(new Employee((uint)row[0], AccountDAO.GetById((uint)row[0]), (string)row[1], row[2].ToString(), (string)row[3], row[4].ToString(),
                Convert.ToChar(row[5]), Convert.ToChar(row[6]), row[7].ToString(), (decimal)row[8], row[9].ToString())));
            return list;
        }

        public static Employee GetById(uint id) {
            var row = db.ReaderRow(db.ReturnCommand($"select LoginId, EmpName, EmpBirthDate, EmpCPF, EmpRG, EmpRG_n, EmpGender, EmpPhoneNumber, EmpSalary, EmpImage from tbEmployee where LoginId = '{id}'"));
            Employee employee = new Employee((uint)row[0], AccountDAO.GetById((uint)row[0]), (string)row[1], row[2].ToString(), (string)row[3], row[4].ToString(),
                Convert.ToChar(row[5]), Convert.ToChar(row[6]), row[7].ToString(), (decimal)row[8], row[9].ToString());
            return employee;
        }
    }
}