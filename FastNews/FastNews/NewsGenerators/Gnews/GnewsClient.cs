using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace FastNews.NewsGenerators.Gnews
{
    internal class GnewsClient
    {
        private const string Token = "659728664a0cb183789e5e29218f8290";
        public async Task<List<string>> GetNewsAsync(string query)
        {
            using (var client = new HttpClient())
            {
                var newsJson = await client.GetStringAsync($"https://gnews.io/api/v4/search?q={query}&token={Token}&lang=en");
                var news = JsonConvert.DeserializeObject<GnewsResponse>(newsJson);
                return news.Articles.Select(n => n.Description).ToArray();
            }
        }
    }
}
