using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Attributes;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Attributes
{
    /// <summary>
    /// Tests the attribute which indicate that a given claim type is required.
    /// </summary>
    [TestFixture]
    public class RequiredClaimTypeAttributeTests
    {
        /// <summary>
        /// Tests that the constructor initialzie an attribute which indicate that a given claim type is required.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeRequiredClaimTypeAttribute()
        {
            var fixture = new Fixture();
            var claimType = fixture.Create<string>();

            var requiredClaimTypeAttribute = new RequiredClaimTypeAttribute(claimType);
            Assert.That(requiredClaimTypeAttribute, Is.Not.Null);
            Assert.That(requiredClaimTypeAttribute.RequiredClaimType, Is.Not.Null);
            Assert.That(requiredClaimTypeAttribute.RequiredClaimType, Is.Not.Empty);
            Assert.That(requiredClaimTypeAttribute.RequiredClaimType, Is.EqualTo(claimType));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the required claim type is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenRequiredClaimTypeIsInvalid(string invalidValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new RequiredClaimTypeAttribute(invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("requiredClaimType"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
