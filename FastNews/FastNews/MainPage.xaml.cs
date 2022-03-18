using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace FastNews
{
    public partial class MainPage : ContentPage
    {
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

        private Xamarin.Forms.Color _textColor = Color.Red;
        public Xamarin.Forms.Color TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                OnPropertyChanged(nameof(TextColor)); // Notify that there was a change on this property
            }
        }

        public MainPage()
        {
            InitializeComponent();
            
            BindingContext = this;
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            var allNews = await GetNews();
            bool primaryColor = true;

            foreach (var news in allNews)
            {
                foreach(var word in news.Split(' '))
                {
                    NewsText = word;
                    await Task.Delay(130);
                }
                TextColor = primaryColor ? Color.Green : Color.Black;
                primaryColor = !primaryColor;
            };
        }

        public async Task<List<string>> GetNews()
        {
            using (HttpClient client = new HttpClient())
            {
                var newsHtml = await client.GetAsync("https://poinformowani.pl/");
                var doc = new HtmlDocument();
                doc.LoadHtml(newsHtml.Content.ReadAsStringAsync().Result);

                List<HtmlNode> allNodes = new List<HtmlNode>();
                var headers = doc.DocumentNode.SelectNodes("//article")
                    .SelectMany(x => GetNodes(x, allNodes))
                    .Where(x => x.Name == "h2")
                    .Select(x => x.InnerText.Replace("&nbsp;", " ").Replace("&quot;", " "))
                    .Distinct()
                    .ToList();

                return headers;
            }

            List<HtmlNode> GetNodes(HtmlNode node, List<HtmlNode> result)
            {
                result.Add(node);

                if (node is null)
                    return result;

                node.ChildNodes.ToList().ForEach(x => GetNodes(x, result));

                return result;
            }
        }
    }
}
