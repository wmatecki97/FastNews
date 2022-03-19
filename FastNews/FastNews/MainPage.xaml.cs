using FastNews.NewsGenerators;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FastNews
{
    public partial class MainPage : ContentPage
    {
        #region controls
        private string _newsText = "";
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

        private bool _filterVisitedNews;
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
        #endregion

        public MainPage()
        {
            InitializeComponent();
            
            BindingContext = this;
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            var generator = new NewsGeneratorsFactory().GetGeneratorByName("Poinformowani");
            var allNews = await generator.GetNews();
            var notVisitedNews = new VisitedNewsHistory().AddNewsToHistory(allNews);
            var newsToDisplay = FilterVisitedNews ? notVisitedNews : allNews;

            bool primaryColor = true;
            foreach (var news in newsToDisplay)
            {
                foreach(var word in news.Split(' '))
                {
                    NewsText = word;
                    await Task.Delay(130);
                }
                TextColor = primaryColor ? Color.Green : Color.Black;
                primaryColor = !primaryColor;
                await Task.Delay(130);
            };
        }
    }
}
