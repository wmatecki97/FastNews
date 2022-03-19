using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastNews.NewsGenerators.Gnews
{
    internal class TechGnewsGenerator : INewsGenerator
    {
        public string ServiceName { get; set; } = "Tech";

        public Task<List<string>> GetNews()
        {
            var client = new GnewsClient();
            return client.GetNewsAsync("tech");
        }
    }
}
