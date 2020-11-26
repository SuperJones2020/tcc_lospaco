namespace TCC_LOSPACO.Models {
    public class Schedule {
        public Employee Employee { get; private set; }
        public Customer Costumer { get; private set; }
        public Service Service { get; private set; }
        public string Date { get; private set; }

        public Schedule(Employee employee, Customer costumer, Service service, string date) {
            Employee = employee;
            Costumer = costumer;
            Service = service;
            Date = date;
        }

        public Schedule() { }
    }
}