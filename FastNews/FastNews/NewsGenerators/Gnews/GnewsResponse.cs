namespace FastNews.NewsGenerators.Gnews
{
    internal class GnewsResponse
    {
        public GnewsArticle[] Articles { get; set; }
    }

    internal class GnewsArticle
    {
        public string Description { get; set; }
    }
}
