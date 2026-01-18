using ShoppingListApp.Models;

namespace ShoppingListApp.Views
{
    public partial class ProductView : ContentView
    {
        public static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(Command<ShoppingItem>), typeof(ProductView));

        public Command<ShoppingItem> DeleteCommand
        {
            get => (Command<ShoppingItem>)GetValue(DeleteCommandProperty);
            set => SetValue(DeleteCommandProperty, value);
        }

        public ProductView()
        {
            InitializeComponent();
        }

        private void OnDecreaseClicked(object sender, EventArgs e)
        {
            if (BindingContext is ShoppingItem item && item.Quantity > 0)
                item.Quantity--;
        }

        private void OnIncreaseClicked(object sender, EventArgs e)
        {
            if (BindingContext is ShoppingItem item)
                item.Quantity++;
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (BindingContext is ShoppingItem item)
                DeleteCommand?.Execute(item);
        }
    }
}