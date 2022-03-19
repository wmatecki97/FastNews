﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastNews.NewsGenerators
{
    internal interface INewsGenerator
    {
        string ServiceName { get; set; }

        Task<List<string>> GetNews();
    }
}