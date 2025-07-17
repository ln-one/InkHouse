using CommunityToolkit.Mvvm.ComponentModel;

namespace InkHouse.ViewModels
{
    public class Recommendation : ObservableObject
    {
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        private bool _isCurrent;
        public bool IsCurrent
        {
            get => _isCurrent;
            set => SetProperty(ref _isCurrent, value);
        }
    }
} 