using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FastNews
{
    internal class VisitedNewsHistory
    {
        public List<string> AddNewsToHistory(List<string> news)
        {
            string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "visited.txt");
            string visitedFileContent = File.Exists(_fileName)
                ? visitedFileContent = File.ReadAllText(_fileName)
                : string.Empty;
            
            var lines = visitedFileContent.Split(new[] { $"{Environment.NewLine}" }, StringSplitOptions.RemoveEmptyEntries);
            var notVisited = news.Except(news).ToList();
            var sb = new StringBuilder(visitedFileContent);
            notVisited.ForEach(x => sb.AppendLine(x));
            visitedFileContent = sb.ToString();
            File.WriteAllText(_fileName, visitedFileContent);

            return notVisited;
        }
    }
}
