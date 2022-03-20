using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastNews.NewsGenerators
{
    public interface INewsGenerator
    {
        Task<List<string>> GetNews();
    }
}
