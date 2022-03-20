using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FastNews.NewsGenerators
{
    internal abstract class ApNewsGeneratorBase : IPredefinedNewsGenerator
    {
        protected abstract string RequestUri { get; }

        public abstract string ServiceName { get; }

        public async Task<int> GetUnreadNewsCount()
        {
            var historyManager = new VisitedNewsHistoryManager();
            List<string> news = await GetNews();
            return historyManager.GetNotVisitedNews(news).Count();
        } 

        private List<string> _news;

        public async Task<List<string>> GetNews()
        {
            if(_news != null)
                return _news;

            using (HttpClient client = new HttpClient())
            {
                var newsHtml = await client.GetAsync(RequestUri);
                var doc = new HtmlDocument();
                doc.LoadHtml(newsHtml.Content.ReadAsStringAsync().Result);
                var allnodes = doc.DocumentNode.Descendants(50);
                var contentNodes = allnodes.Where(n => n.GetClasses().Any(c => c == "content") && n.GetClasses().Any(c => c.StartsWith("text-")))
                    .Select(div => div.FirstChild.InnerText)
                    .ToList();

                _news = contentNodes;
                return contentNodes;
            }
        }
    }
}
