using System;
using System.Linq;
using System.Reflection;

namespace FastNews.NewsGenerators
{
    public class NewsGeneratorsFactory
    {
        public INewsGenerator GetGeneratorByName(string name)
        {
            var generators = Assembly.GetCallingAssembly().GetTypes().Where(t => t.IsAssignableFrom(typeof(INewsGenerator)));
            var availableGenerators = generators.Select(g => (INewsGenerator)Activator.CreateInstance(g)).ToArray();
            return availableGenerators.FirstOrDefault(g => g.ServiceName == name);
        }
    }
}
