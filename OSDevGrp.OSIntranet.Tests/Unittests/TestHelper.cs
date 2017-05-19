using System;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests
{
    /// <summary>
    /// Test helper to make unit testing easy.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Asserts that an ArgumentNullException is valid.
        /// </summary>
        /// <param name="argumentNullException">The ArgumentNullException to assert on.</param>
        /// <param name="expectedParamName">The expected parameter name which should be null.</param>
        public static void AssertArgumentNullExceptionIsValid(ArgumentNullException argumentNullException, string expectedParamName)
        {
            if (expectedParamName == null)
            {
                throw new ArgumentNullException(nameof(expectedParamName));
            }

            Assert.That(argumentNullException, Is.Not.Null);
            Assert.That(argumentNullException.ParamName, Is.Not.Null);
            Assert.That(argumentNullException.ParamName, Is.Not.Empty);
            Assert.That(argumentNullException.ParamName, Is.EqualTo(expectedParamName));
            Assert.That(argumentNullException.InnerException, Is.Null);
        }
    }
}
