using System;
using System.IO;
using NUnit.Framework;

namespace ELMPrototype
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var dir = Path.GetDirectoryName(typeof(Setup).Assembly.Location);
            Environment.CurrentDirectory = dir;
        }
    }
}