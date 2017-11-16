using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Vakta.Core;

namespace Vakta.Tests
{
    [TestFixture]
    public class Class1Tests
    {
        [Test]
        public async Task GetTop5HackerNewsItems()
        {
            string assemblyFile = 
                (new System.Uri(Assembly.GetExecutingAssembly().CodeBase))
                .AbsolutePath;

            var dirName = Path.GetDirectoryName(assemblyFile);

            // Go up two folders from bin/Debug
            for (var i = 0; i < 2; i++)
                dirName = Path.GetDirectoryName(dirName);

            var apiKey = File.ReadAllText(
                Path.Combine(dirName, "summaryapikey.secrets.txt"));

            var options = new RunOptions(
                @"c:\temp",
                "http://api.smmry.com/?SM_API_KEY={0}&SM_LENGTH=2&SM_URL={1}",
                apiKey);

            var result = await Class1.Run(options);

            Assert.AreEqual(5, result.Count);
        }
    }
}
