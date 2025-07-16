using Avalonia.Controls;
using Avalonia.Interactivity;
using InkHouse.ViewModels;

namespace InkHouse.Views
{
    public partial class SeatReservationView : UserControl
    {
        public SeatReservationView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is SeatReservationViewModel vm && vm.RefreshCommand.CanExecute(null))
            {
                await vm.RefreshCommand.ExecuteAsync(null);
            }
        }
    }
}