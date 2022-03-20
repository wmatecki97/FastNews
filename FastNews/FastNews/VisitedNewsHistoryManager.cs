using FastNews.NewsGenerators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastNews
{
    internal class VisitedNewsHistoryManager
    {
        private static string _fileName => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "visited.txt");

        private string _visitedFileContent;

        private string VisitedFileContent
        {
            get => _visitedFileContent ?? (_visitedFileContent = File.Exists(_fileName) ? File.ReadAllText(_fileName) : string.Empty);
            set => _visitedFileContent = value;
        }

        public List<string> AddNewsToHistory(params string[] news)
        {
            var notVisited = GetNotVisitedNews(news);

            var sb = new StringBuilder(VisitedFileContent);
            notVisited.ForEach(x => sb.AppendLine(x));
            VisitedFileContent = sb.ToString();
            File.WriteAllText(_fileName, VisitedFileContent);

            return notVisited;
        }

        public List<string> GetNotVisitedNews(IEnumerable<string> news)
        {
            var lines = VisitedFileContent.Split(new[] { $"{Environment.NewLine}" }, StringSplitOptions.RemoveEmptyEntries);
            var notVisited = news.Except(lines).ToList();
            return notVisited;
        }

        public async Task<int> GetUnreadNewsCountAsync(INewsGenerator generator)
        {
            var news = await generator.GetNews();
            var notVisited = GetNotVisitedNews(news);
            return notVisited.Count();
        }
    }
}
