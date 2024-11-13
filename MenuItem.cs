using System;

namespace EmployeeList
{
    public class MenuItem
    {
        //Ominaisuudet
        public int Id { get; set; }
        public string Name { get; set; }
        //Tapahtuma
        public event EventHandler<MenuItemEventArgs> ItemSelected;
        public void Select()
        {
            MenuItemEventArgs temp = new MenuItemEventArgs() { ItemId = Id };
            ItemSelected(this, temp);
        }
        // Metodi joka ylikirjoittaa ToString metodin
        public override string ToString() => $"{Id} {Name}";
    }
}
