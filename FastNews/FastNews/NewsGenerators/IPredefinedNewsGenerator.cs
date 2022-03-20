using System.Threading.Tasks;

namespace FastNews.NewsGenerators
{
    internal interface IPredefinedNewsGenerator : INewsGenerator
    {
        string ServiceName { get; }
        Task<int> GetUnreadNewsCount();
    }
}