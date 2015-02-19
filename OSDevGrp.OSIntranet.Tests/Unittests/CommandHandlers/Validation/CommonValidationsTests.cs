using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Validation
{
    /// <summary>
    /// Tests the common validations.
    /// </summary>
    [TestFixture]
    public class CommonValidationsTests
    {
        /// <summary>
        /// Tests that the constructor initialize the common validations.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeCommonValidations()
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);
        }
    }
}
