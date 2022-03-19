using FastNews.NewsGenerators;
using FastNews.NewsGenerators.ApNews;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NewsTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GeneratorsFactory_GetByName_ReturnsInstanceOfGenerator()
        {
            var factory = new NewsGeneratorsFactory();
            var c = factory.GetGeneratorByName("Poinformowani");
            Assert.IsNotNull(c);
        }

        [Test]
        public async Task GeneratorsFactory_GetByName_ReturnsInstanceOfGenerator2()
        {
            var generator = new ApTechNewsGenerator();
            var c = await generator.GetNews();
            Assert.IsNotNull(c);
        }
    }
}