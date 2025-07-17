using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Timers;
using Avalonia.Threading;

namespace InkHouse.ViewModels
{
    public class BookRecommendationViewModel : ViewModelBase
    {
        public ObservableCollection<Recommendation> Recommendations { get; }
        private int _currentIndex;
        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (value != _currentIndex)
                {
                    if (_currentIndex >= 0 && _currentIndex < Recommendations.Count)
                        Recommendations[_currentIndex].IsCurrent = false;
                    _currentIndex = value;
                    if (_currentIndex >= 0 && _currentIndex < Recommendations.Count)
                        Recommendations[_currentIndex].IsCurrent = true;
                    OnPropertyChanged(nameof(CurrentRecommendation));
                }
            }
        }
        public Recommendation CurrentRecommendation =>
            (CurrentIndex >= 0 && CurrentIndex < Recommendations.Count) ? Recommendations[CurrentIndex] : null;

        private Timer _timer;

        public BookRecommendationViewModel()
        {
            Recommendations = new ObservableCollection<Recommendation>
            {
                new Recommendation
                {
                    ImagePath = "avares://InkHouse/Assets/Recommendations/hongloumeng.jpg",
                    Title = "红楼梦",
                    Author = "曹雪芹",
                    Description = "中国古典小说巅峰之作，描绘贾宝玉与林黛玉的悲欢离合。"
                },
                new Recommendation
                {
                    ImagePath = "avares://InkHouse/Assets/Recommendations/hamuleite.jpg",
                    Title = "哈姆雷特",
                    Author = "莎士比亚",
                    Description = "世界著名悲剧，探讨人性、复仇与命运。"
                },
                new Recommendation
                {
                    ImagePath = "avares://InkHouse/Assets/Recommendations/lunyu.jpg",
                    Title = "论语",
                    Author = "孔子弟子",
                    Description = "儒家经典，记录孔子及其弟子的言行思想。"
                },
                new Recommendation
                {
                    ImagePath = "avares://InkHouse/Assets/Recommendations/beicanshijie.jpg",
                    Title = "悲惨世界",
                    Author = "雨果",
                    Description = "法国文学巨著，展现苦难与救赎的壮丽史诗。"
                }
            };
            _currentIndex = 0;
            if (Recommendations.Count > 0)
                Recommendations[0].IsCurrent = true;
            _timer = new Timer(3000);
            _timer.Elapsed += (s, e) => Next();
            _timer.AutoReset = true;
            _timer.Start();
        }

        public void Next()
        {
            if (Recommendations.Count == 0) return;
            Dispatcher.UIThread.Post(() =>
            {
                CurrentIndex = (CurrentIndex + 1) % Recommendations.Count;
            });
        }
    }
}
