using Avalonia.Controls;
using InkHouse.ViewModels;

namespace InkHouse.Views
{
    public partial class BookRecommendationView : UserControl
    {
        public BookRecommendationView()
        {
            InitializeComponent();
            DataContext = new BookRecommendationViewModel();
        }
    }
} 