using FastNews.NewsGenerators;
using FastNews.NewsGenerators.Gnews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FastNews
{
    public partial class MainPage : ContentPage
    {
        #region controls
        private string _newsText = "Select source below";
        public string NewsText
        {
            get { return _newsText; }
            set
            {
                _newsText = value;
                OnPropertyChanged(nameof(NewsText)); // Notify that there was a change on this property
            }
        }

        private Color _textColor = Color.Red;
        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                OnPropertyChanged(nameof(TextColor)); // Notify that there was a change on this property
            }
        }

        private bool _filterVisitedNews = true;
        public bool FilterVisitedNews
        {
            get
            {
                return _filterVisitedNews;
            }
            set
            {
                _filterVisitedNews = value;
                this.OnPropertyChanged(nameof(FilterVisitedNews));
            }
        }

        private int currentlyWorkingGenerators = 0;
        private bool _areNewsCurrentlyDisplayed = false;
        public bool AreNewsCurrentlyDisplayed
        {
            get
            {
                return _areNewsCurrentlyDisplayed;
            }
            set
            {
                _areNewsCurrentlyDisplayed = value;
                this.OnPropertyChanged(nameof(AreNewsCurrentlyDisplayed));
            }
        }

        private string _customTopicSearch;
        public string CustomTopicSearch
        {
            get
            {
                return _customTopicSearch;
            }
            set
            {
                _customTopicSearch = value;
                this.OnPropertyChanged(nameof(CustomTopicSearch));
            }
        }

        private int _speedValue = 100;
        private const int MaxDelayBetweenWords = 300;
        public int SpeedValue
        {
            get
            {
                return _speedValue;
            }
            set
            {
                _speedValue = value;
                this.OnPropertyChanged(nameof(SpeedValue));
                this.OnPropertyChanged(nameof(Wpm));
            }
        }
        public int DelayBetweenWords => MaxDelayBetweenWords - SpeedValue;

        public string Wpm
        {
            get
            {
                return $"Current reading speed: {1000*60/ DelayBetweenWords} WPM";
            }
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        #endregion

        ObservableCollection<String> list;

        public ICommand RefreshCommand { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FillPredefinedGeneratorsList();
        }

        private async Task FillPredefinedGeneratorsList()
        {
            var history = new VisitedNewsHistoryManager();
            var predefinedGenerators = new List<string>();
            list = new ObservableCollection<string>(predefinedGenerators);
            listView.ItemsSource = list;
            var updateGeneratorsListTasks = NewsGeneratorsFactory.GetPredefinedGenerators().Select(generator => Task.Run(async () =>
            {
                int unreadedNewsCount = await history.GetUnreadNewsCountAsync(generator);
                var displayLabel = GetPredefinedGeneratorDisplayLabel(generator, unreadedNewsCount);
                lock (list)
                {
                    list.Add(displayLabel);
                }
            })).ToArray();

            await Task.WhenAll(updateGeneratorsListTasks);
        }

        private static string GetPredefinedGeneratorDisplayLabel(IPredefinedNewsGenerator generator, int unreadedNewsCount)
        {
            return $"{generator.ServiceName} ({unreadedNewsCount})";
        }

        public MainPage()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand);
            InitializeComponent();
            BindingContext = this;
        }

        private async void ExecuteRefreshCommand(object obj)
        {
            await FillPredefinedGeneratorsList();
            IsRefreshing = false;
        }

        async void ShowCustomTopicNewsClicked(object sender, EventArgs args)
        {
            var generator = new GnewsGenerator(CustomTopicSearch);
            await ShowNews(generator);
        }

        public async void GetNewsFromPredefinedSource(Object Sender, EventArgs args)
        {
            Button button = (Button)Sender;
            _currentTaskToken = Guid.NewGuid();
            
            string selectedGeneratorLabel = button.Text;
            var generator = new NewsGeneratorsFactory().GetGeneratorByName(selectedGeneratorLabel);
            var unreadCount = await generator.GetUnreadNewsCount();
            await ShowNews(generator, () => button.Text = GetPredefinedGeneratorDisplayLabel(generator, unreadCount--));
        }

        private Guid _currentTaskToken { get; set; }

        private async Task ShowNews(INewsGenerator generator, Action AfterEveryNewsAction = null)
        {
            var token = _currentTaskToken;
            var allNews = await generator.GetNews();
            var visitedNewsHistoryManager = new VisitedNewsHistoryManager();
            var notVisitedNews = visitedNewsHistoryManager.GetNotVisitedNews(allNews);
            var newsToDisplay = FilterVisitedNews ? notVisitedNews : allNews;
            bool primaryColor = true;

            Interlocked.Increment(ref currentlyWorkingGenerators);
            if (currentlyWorkingGenerators > 0)
                AreNewsCurrentlyDisplayed = true;

            foreach (var news in newsToDisplay)
            {
                NewsText = String.Empty;
                TextColor = primaryColor ? Color.Green : Color.Black;
                primaryColor = !primaryColor;
                foreach (var word in news.Split(' '))
                {
                    NewsText = word;
                    await Task.Delay(DelayBetweenWords);
                }
                visitedNewsHistoryManager.AddNewsToHistory(news);

                await Task.Delay(DelayBetweenWords);
                
                if (AfterEveryNewsAction != null)
                {
                    AfterEveryNewsAction();
                }

                if (_currentTaskToken != token)
                    break;
            };

            Interlocked.Decrement(ref currentlyWorkingGenerators);
            if (currentlyWorkingGenerators == 0)
                AreNewsCurrentlyDisplayed = false;
        }

        private void StopNews(object sender, EventArgs e)
        {
            _currentTaskToken = Guid.NewGuid();
        }
    }
}
