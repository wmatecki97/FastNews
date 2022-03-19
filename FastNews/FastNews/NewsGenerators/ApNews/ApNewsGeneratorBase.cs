using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FastNews.NewsGenerators
{
    internal abstract class ApNewsGeneratorBase : INewsGenerator
    {
        protected abstract string RequestUri { get; }

        public abstract string ServiceName { get; }

        public async Task<List<string>> GetNews()
        {
            using (HttpClient client = new HttpClient())
            {
                var newsHtml = await client.GetAsync(RequestUri);
                var doc = new HtmlDocument();
                doc.LoadHtml(newsHtml.Content.ReadAsStringAsync().Result);
                var allnodes = doc.DocumentNode.DescendantNodes(50);
                var contentNodes = allnodes.Where(n => n.GetClasses().Any(c => c == "content") && n.GetClasses().Any(c => c.StartsWith("text-")))
                    .Select(div => div.FirstChild.InnerText)
                    .ToList();

                return contentNodes;
            }
        }
    }
}
