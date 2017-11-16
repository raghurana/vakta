using NUnit.Framework;
using Vakta.Core;

namespace Vakta.Tests
{
    [TestFixture]
    public class Class1Tests
    {
        [Test]
        public void GetTop5HackerNewsItems()
        {
            var sut = new Class1();
            var links = sut.GetItems(5);
        }
    }
}
