using System;
using System.Linq;
using System.Reflection;

namespace FastNews.NewsGenerators
{
    internal class NewsGeneratorsFactory
    {
        internal IPredefinedNewsGenerator GetGeneratorByName(string name)
        {
            IPredefinedNewsGenerator[] availableGenerators = GetPredefinedGenerators();
            return availableGenerators.FirstOrDefault(g => name.StartsWith(g.ServiceName));
        }

        public static IPredefinedNewsGenerator[] GetPredefinedGenerators()
        {
            var generators = Assembly.GetAssembly(typeof(NewsGeneratorsFactory)).GetTypes()
                            .Where(t => t.IsClass && !t.IsAbstract && typeof(IPredefinedNewsGenerator).IsAssignableFrom(t));
            var availableGenerators = generators.Select(g => (IPredefinedNewsGenerator)Activator.CreateInstance(g)).ToArray();
            return availableGenerators;
        }
    }
}
