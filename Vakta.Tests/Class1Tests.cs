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
            var sut = new Class1();
            var links = await sut.GetItems(5);

            Assert.AreEqual(5, links.Count);
        }
    }
}
