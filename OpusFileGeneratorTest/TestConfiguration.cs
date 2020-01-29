using Microsoft.Extensions.Configuration;
using System;

namespace OpusFileGeneratorTest
{
    static class TestConfiguration
    {
        public static IConfiguration Get()
        {
            return new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
        }
    }
}
