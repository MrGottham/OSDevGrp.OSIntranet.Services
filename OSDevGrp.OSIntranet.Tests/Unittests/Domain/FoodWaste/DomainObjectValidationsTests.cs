using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the common validations used by domain objects in the food waste domain.
    /// </summary>
    [TestFixture]
    public class DomainObjectValidationsTests
    {
        /// <summary>
        /// Tests that the constructor initialize common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDomainObjectValidations()
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);
        }

        /// <summary>
        /// Tests that IsMailAddress throws ArgumentNullException when the value for the mail address is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatIsMailAddressThrowsArgumentNullExceptionWhenMailAddressIsNullOrEmpty(string validMailAddress)
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => domainObjectValidations.IsMailAddress(validMailAddress));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsMailAddress returns true for valid mail addresses.
        /// </summary>
        [Test]
        [TestCase("mrgottham@gmail.com")]
        [TestCase("test@osdevgrp.dk")]
        [TestCase("ole.sorensen@visma.com")]
        public void TestThatIsMailAddressReturnsTrueForValidMailAddress(string validMailAddress)
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            var result = domainObjectValidations.IsMailAddress(validMailAddress);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that IsMailAddress returns false for invalid mail addresses.
        /// </summary>
        [Test]
        [TestCase("XYZ")]
        [TestCase("XYZ.XXX")]
        [TestCase("XYZ.XXX.COM")]
        public void TestThatIsMailAddressReturnsFalseForInvalidMailAddress(string validMailAddress)
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            var result = domainObjectValidations.IsMailAddress(validMailAddress);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that Create initialize common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatCreateInitializeDomainObjectValidations()
        {
            var domainObjectValidations = DomainObjectValidations.Create();
            Assert.That(domainObjectValidations, Is.Not.Null);
            Assert.That(domainObjectValidations, Is.TypeOf<DomainObjectValidations>());
        }
    }
}
