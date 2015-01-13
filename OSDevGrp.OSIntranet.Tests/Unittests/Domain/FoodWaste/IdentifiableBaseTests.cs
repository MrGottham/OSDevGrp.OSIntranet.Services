using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the basic functionality for an identifiable domain object in the food waste domain.
    /// </summary>
    [TestFixture]
    public class IdentifiableBaseTests
    {
        /// <summary>
        /// Private class for testing the basic functionality for an identifiable domain object in the food waste domain.
        /// </summary>
        private class MyIdentifiable : IdentifiableBase
        {
        }

        /// <summary>
        /// Tests that the constructor initialize basic functionality for an identifiable domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeIdentifiableBase()
        {
            var identifiable = new MyIdentifiable();
            Assert.That(identifiable, Is.Not.Null);
            Assert.That(identifiable.Identifier, Is.Null);
            Assert.That(identifiable.Identifier.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that the setter for Identifier sets the identifier.
        /// </summary>
        [Test]
        [TestCase("{B391DCA5-688C-402D-850C-EC6337BE139A}")]
        [TestCase("{676D17C0-A62C-4A4E-AD2F-14C4F5CF8DF2}")]
        [TestCase("{1D03F532-AF0D-4F59-93F0-8A6DDA2EC70A}")]
        public void TestThatIdentifierSetterSetsIdentififer(string identifier)
        {
            var id = new Guid(identifier);

            var identifiable = new MyIdentifiable();
            Assert.That(identifiable, Is.Not.Null);
            Assert.That(identifiable.Identifier, Is.Null);
            Assert.That(identifiable.Identifier.HasValue, Is.False);

            identifiable.Identifier = id;
            Assert.That(identifiable, Is.Not.Null);
            Assert.That(identifiable.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(identifiable.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }
    }
}
