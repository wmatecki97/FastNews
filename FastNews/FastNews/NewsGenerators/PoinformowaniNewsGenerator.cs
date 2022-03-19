using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FastNews.NewsGenerators
{
    internal class PoinformowaniNewsGenerator : INewsGenerator
    {
        public string ServiceName { get; set; } = "Poinformowani";

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
