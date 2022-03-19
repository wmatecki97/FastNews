using System;
using System.Linq;
using System.Reflection;

namespace FastNews.NewsGenerators
{
    internal class NewsGeneratorsFactory
    {
        internal INewsGenerator GetGeneratorByName(string name)
        {
            var generators = Assembly.GetAssembly(typeof(NewsGeneratorsFactory)).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(INewsGenerator).IsAssignableFrom(t));
            var availableGenerators = generators.Select(g => (INewsGenerator) Activator.CreateInstance(g)).ToArray();
            return availableGenerators.FirstOrDefault(g => g.ServiceName == name);
        }
    }
}
