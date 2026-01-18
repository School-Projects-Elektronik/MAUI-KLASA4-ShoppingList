using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShoppingListApp.Models
{
    public class ShoppingItem : INotifyPropertyChanged
    {
        private string name;
        private string unit;
        private int quantity;
        private bool isBought;

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        public string Unit
        {
            get => unit;
            set { unit = value; OnPropertyChanged(); }
        }

        public int Quantity
        {
            get => quantity;
            set { quantity = Math.Max(0, value); OnPropertyChanged(); }
        }

        public bool IsBought
        {
            get => isBought;
            set { isBought = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}