using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastNews.NewsGenerators.Gnews
{
    internal class GnewsGenerator : INewsGenerator
    {
        public string CustomSearchValue { get; }

        public GnewsGenerator()
        {
        }

        public GnewsGenerator(string customSearchValue)
        {
            CustomSearchValue = customSearchValue;
        }

        public Task<List<string>> GetNews()
        {
            var client = new GnewsClient();
            return client.GetNewsAsync(CustomSearchValue);
        }
    }
}
