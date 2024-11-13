using System;

namespace EmployeeList
{
    public class Employee
    {
        //Kenttämuuttuja
        private double _salary;
        //Ominaisuudet
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Department Department { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Name => $"{LastName} {FirstName}";
        public int Age {
            get
            {//Lasketaan syntymäpäivän avulla ikä jos ikää ei ole palautetaan 0
                if (DateOfBirth == null)
                {
                    return 0;
                }
                else
                {
                    int age = DateTime.Today.Year - DateOfBirth.Value.Year;
                    if (DateOfBirth.Value.Month > DateTime.Today.Month ||
                        (DateOfBirth.Value.Month == DateTime.Today.Month && DateOfBirth.Value.Day > DateTime.Today.Day))
                    {
                        age--;
                    }
                    return age;
                }
            }
        }
        public double Salary
        {
            //palautetaan palkka pyöristettynä kahteen desimaaliin
            get { return Math.Round(_salary,2); }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Negatiivinen palkka");
                }
                _salary = value;
            }
        }
        //Konstruktori
        public Employee(int id, string first, string last, DateTime dob,double salary)
        {
            Id = id;
            FirstName = first;
            LastName = last;
            DateOfBirth = dob;
            Salary = salary;
            StartDate = DateTime.Now;
            EndDate = null;
        }
        // Metodi joka ylikirjoittaa ToString metodin
        public override string ToString() => $"{Id} {FirstName} {LastName}";

    }
}
