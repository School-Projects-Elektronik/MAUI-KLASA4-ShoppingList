using System.Collections.ObjectModel;
using System.Text.Json;
using ShoppingListApp.Models;

namespace ShoppingListApp.Views
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<ShoppingItem> ShoppingItems { get; set; } = new();
        public Command<ShoppingItem> DeleteProductCommand { get; set; }
        private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "shopping_data.json");

        public MainPage()
        {
            InitializeComponent();
            DeleteProductCommand = new Command<ShoppingItem>(DeleteProduct);
            LoadData();
            ItemsCollectionView.ItemsSource = ShoppingItems;
            BindingContext = this;
        }

        private void OnAddItemClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text)) return;

            int.TryParse(QuantityEntry.Text, out int qty);
            var newItem = new ShoppingItem
            {
                Name = NameEntry.Text,
                Unit = UnitEntry.Text,
                Quantity = qty > 0 ? qty : 1,
                IsBought = false
            };

            newItem.PropertyChanged += OnItemPropertyChanged;

            ShoppingItems.Add(newItem);
            SortList();
            SaveData();
            ClearForm();
        }

        private void DeleteProduct(ShoppingItem item)
        {
            ShoppingItems.Remove(item);
            SaveData();
        }

        private void OnItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShoppingItem.IsBought))
            {
                SortList();
            }
            SaveData();
        }

        private void SortList()
        {
            var sorted = ShoppingItems.OrderBy(i => i.IsBought).ToList();
            for (int i = 0; i < sorted.Count; i++)
            {
                int oldIndex = ShoppingItems.IndexOf(sorted[i]);
                if (oldIndex != i) ShoppingItems.Move(oldIndex, i);
            }
        }

        private void ClearForm()
        {
            NameEntry.Text = string.Empty;
            UnitEntry.Text = string.Empty;
            QuantityEntry.Text = string.Empty;
        }

        private void SaveData()
        {
            try
            {
                string json = JsonSerializer.Serialize(ShoppingItems);
                File.WriteAllText(filePath, json);
            }
            catch { }
        }

        private void LoadData()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var items = JsonSerializer.Deserialize<List<ShoppingItem>>(json);
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            item.PropertyChanged += OnItemPropertyChanged;
                            ShoppingItems.Add(item);
                        }
                        SortList();
                    }
                }
                catch { }
            }
        }
    }
}