using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using System.Reflection;


namespace EmployeeList
{
    public static class Application
    {
        
        static List<MenuItem> Menu;
        //
        static void WriteResult<T>(int itemid, List<T> result)
        {
            string row;
            //otsikkorivit
            WriteLine();
            WriteLine(Menu.Where(mi=>mi.Id==itemid).First().Name.ToUpper());
            WriteLine("-".PadRight(18*result[0].GetType().GetProperties().Length+2,'-'));
            //sarakeotsikkorivit
            if(result.Count>0)
            {
                row="";
                foreach(PropertyInfo property in result[0].GetType().GetProperties())
                {
                    row+=$"{property.Name}".PadRight(16)+"|";
                }
                WriteLine(row);
            }
            WriteLine("-".PadRight(18*result[0].GetType().GetProperties().Length+2,'-'));
            //datarivit
            foreach(object item in result)
            {
                row="";
                foreach(PropertyInfo property in item.GetType().GetProperties())
                { 
                    row+=$"{property.GetValue(item,null).ToString()}".PadRight(16)+"|";}WriteLine(row);
            }
            WriteLine("-".PadRight(18*result[0].GetType().GetProperties().Length+2,'-'));
            WriteLine();
            Write("Paina Enter jatkaaksesi.");
            ReadLine();
        }
        // Metodi InitializeMenu joka alustaa oliot ja lisää ne listaan
        // jonka jälkeen alustetaan tapahtumat käyttäen Linq:ta
        static void InitializeMenu()
        {
            Menu = new List<MenuItem>();
            MenuItem menu1 = new MenuItem() { Id = 1, Name = "50 vuotiaat työntekijät" };
            MenuItem menu2 = new MenuItem() { Id = 2, Name = "Osastot yli 50 henkilöä" };
            MenuItem menu3 = new MenuItem() { Id = 3, Name = "Sukunimen työtekijät" };
            MenuItem menu4 = new MenuItem() { Id = 4, Name = "Osastojen isoimmat palkat" };
            MenuItem menu5 = new MenuItem() { Id = 5, Name = "Viisi yleisintä sukunimeä" };
            MenuItem menu6 = new MenuItem() { Id = 6, Name = "Osastojen ikäjakaumat" };
            Menu.Add(menu1);
            Menu.Add(menu2);
            Menu.Add(menu3);
            Menu.Add(menu4);
            Menu.Add(menu5);
            Menu.Add(menu6);

            //Tapahtumakäsittelijät
            Menu[0].ItemSelected += (obj, a) => {
                var result = Data.Employees
                .Where(e => e.Age == 50)
                .Select(e => new { Nimi = e.Name, ika = e.Age, osasto = e.Department.Name }) ;
                WriteResult(a.ItemId, result.ToList());
            };
            Menu[1].ItemSelected += (obj, a) => {
                var result = Data.Departments
                .Where(d => d.EmployeeCount > 50)
                .OrderByDescending(d=> d.EmployeeCount)
                .Select(d => new {Id = d.Id, Nimi = d.Name, Vahvuus = d.EmployeeCount });
                WriteResult(a.ItemId, result.ToList());
            };
            Menu[2].ItemSelected += (obj, a) => {
                Write("\nAnna sukunimi:");
                string b = ReadLine();
                var result = Data.Employees
                .Where(e => e.LastName == b)
                .Select(e => new { id = e.Id, nimi = e.Name });
                WriteResult(a.ItemId, result.ToList()); 
            };
            Menu[3].ItemSelected += (obj, a) => {
                var result = Data.Departments
                .SelectMany(d => d.Employees, 
                    (d, e) => new {
                    Osasto = d.Name,
                    Palkka = e.Salary })
                .GroupBy(d => d.Osasto)
                .Select(a => new { Osasto = a.Key, Maksimipalkka = a.Max(b => b.Palkka)}
                );
                   WriteResult(a.ItemId, result.ToList());
            };
            Menu[4].ItemSelected += (obj, a) => {
                var result = Data.Employees
                .Where(e => e.LastName != null)
                .GroupBy(e => e.LastName)
                .Select(e => new { Sukunimi = e.Key, Lkm = e.Count() })
                .OrderByDescending(e => e.Lkm)
                .Take(5);
                WriteResult(a.ItemId, result.ToList());
            };
            Menu[5].ItemSelected += (obj, a) => {
                var result = Data.Departments.Select
                (d => new 
                { 
                    Osasto = d.Name,
                    Alle30v =d.Employees.Where(a => a.Age < 30).Count(),
                    valilla30v_50v = d.Employees.Where(b => b.Age > 30 && b.Age < 50).Count(),
                    yli50v = d.Employees.Where(c => c.Age > 50).Count() 
                });
                WriteResult(a.ItemId, result.ToList());
            };
        }
        //Generoidaan data ja kutsutaan Menua
        static void Initialize()
        {
            Data.GenerateData();
            InitializeMenu();
        }
        //Kysytään käyttäjältä kelvollinen syöte
        static int ReadFromMenu()
        {
            Clear();
            WriteLine("Vaihtoehdot:");
            foreach (var item in Menu)
            {
                WriteLine($"{item}");
            }
            Write("valitse (0=lopettaa)");
            string a = ReadLine();
            if ("0123456".Contains(a))
            {
                return int.Parse(a);
            }
            else
            {
                throw new Exception("Syöte ei ole kelvollinen");
            }
        }
        //ajetaan ohjelma
        public static void Run()
        {
            int a;
            Initialize();
            do
            {
                try
                {
                    a = ReadFromMenu();
                    if (a == 0)
                    {
                        break;
                    }
                    a -= 1;
                    Menu[a].Select();
                }
                catch (Exception e)
                {
                    WriteLine($"{e.Message}");
                    ReadKey();
                }
            } while (true);
        }
    }
}
