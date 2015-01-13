using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the basic functionality for a domain object in the food waste domain.
    /// </summary>
    [TestFixture]
    public class DomainObjectBaseTests
    {
        /// <summary>
        /// Private class for testing the basic functionality on a domain object in the food waste domain.
        /// </summary>
        private class MyDomainObject : DomainObjectBase
        {
        }

        /// <summary>
        /// Tests that the constructor initialize the basic functionality for a domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDomainObjectBase()
        {
            var domainObjectBase = new MyDomainObject();
            Assert.That(domainObjectBase, Is.Not.Null);
        }
    }
}
