using System;
using System.Collections.Generic;
using System.Text;

namespace FastNews.NewsGenerators.ApNews
{
    internal class ApScienceNewsGenerator : ApNewsGeneratorBase
    {
        public override string ServiceName => "AP science news";

        protected override string RequestUri => "https://apnews.com/hub/science";
    }

    internal class ApTechNewsGenerator : ApNewsGeneratorBase
    {
        public override string ServiceName => "AP Tech news";

        protected override string RequestUri => "https://apnews.com/hub/technology";
    }

    internal class ApWorldNewsNewsGenerator : ApNewsGeneratorBase
    {
        public override string ServiceName => "AP world news";

        protected override string RequestUri => "https://apnews.com/hub/world-news";
    }

    internal class ApBusinessNewsGenerator : ApNewsGeneratorBase
    {
        public override string ServiceName => "AP business news";

        protected override string RequestUri => "https://apnews.com/hub/business";
    }
}
