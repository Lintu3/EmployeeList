using System.Collections.Generic;


namespace EmployeeList
{
    public class Department
    {
        //Ominaisuudet
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
        public int EmployeeCount => Employees.Count;
        //Konstruktorit
        public Department()
        {
            Employees = new List<Employee>();
        }
        public Department(int id, string name)
            : this()
        {
            Id = id;
            Name = name;
        }
        // Metodi joka ylikirjoittaa ToString metodin
        public override string ToString() => $"{Name} ({EmployeeCount})";
        
    }
}
