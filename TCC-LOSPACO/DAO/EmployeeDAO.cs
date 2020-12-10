using System;
using System.Collections.Generic;
using System.Linq;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public class EmployeeDAO {
        private static Database db = new Database();
        public static List<Employee> GetList() {
            var list = new List<Employee>();
            string query = "select * from tbEmployee";
            db.ReaderRows(db.ReturnCommand(query), row => {
                byte[] bytes = row[5] == DBNull.Value ? new byte[0] : (byte[])row[5];
                list.Add(new Employee(CustomerDAO.GetById((uint)row[0]), row[1] + "", row[2] + "", (decimal)row[3], Convert.ToChar(row[4]), bytes, GetServices((uint)row[0])));
            });
            return list;
        }

        public static List<Service> GetServices(uint id) {
            var list = new List<Service>();
            string query = $"select * from tbservemployees where LoginId = '{id}'";
            db.ReaderRows(db.ReturnCommand(query), row => list.Add(ServiceDAO.GetById((ushort)row[1])));
            return list;
        }

        public static void InsertService(uint id, params ushort[] servId) {
            string query = $"insert into tbservemployees(servid, loginid) values";
            List<ushort> list = servId.ToList();
            list.ForEach(x => {
                int index = list.IndexOf(x);
                query += index != list.Count() - 1 ? $"('{x}', '{id}')," : $"('{x}', '{id}')";
            });
            db.ExecuteCommand(query);
        }

        public static void RemoveService(uint id, ushort servId) {
            string query = $"delete from tbservemployees where loginid='{id}' and servid='{servId}'";
            db.ExecuteCommand(query);
        }

        public static Employee GetById(uint id) {
            var row = db.ReaderRow(db.ReturnCommand($"select * from tbEmployee where LoginId = '{id}'"));
            byte[] bytes = row[5] == DBNull.Value ? new byte[0] : (byte[])row[5];
            return new Employee(CustomerDAO.GetById((uint)row[0]), row[1] + "", (string)row[2], (decimal)row[3], Convert.ToChar(row[4]), bytes, GetServices((uint)row[0]));
        }

        public static Employee GetByRG(string rg) {
            var row = db.ReaderRow(db.ReturnCommand($"select * from tbEmployee where emprg = '{rg}'"));
            byte[] bytes = row[5] == DBNull.Value ? new byte[0] : (byte[])row[5];
            return new Employee(CustomerDAO.GetById((uint)row[0]), row[1] + "", (string)row[2], (decimal)row[3], Convert.ToChar(row[4]), bytes, GetServices((uint)row[0]));
        }

        public static void Insert(string full_name, string username, string email, string password, string number, string birth, string rg, string cpf, string salary, string genre, string image, string services) {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, 12);
            db.ExecuteProcedure("sp_InsertEmployee", email, passwordHash, full_name, username, birth, cpf, rg, salary, genre, number, image);
            ushort[] servIds = services.Split(',').ToList().Select(x => Convert.ToUInt16(x)).ToArray();
            InsertService(GetByRG(rg).Customer.Account.Id, servIds);
        }

        public static List<Employee> GetEmployeesAvaible(string datetime, uint servid) {
            var list = new List<Employee>();
            db.ReaderRows(db.ReturnProcedure("sp_GetEmpsAvailable", datetime, servid), row => {
                uint id = (uint)row[1];
                Employee emp = GetById(id);
                list.Add(emp);
            });
            return list;
        }
    }
}