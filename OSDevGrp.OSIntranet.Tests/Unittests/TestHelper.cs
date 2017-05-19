using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

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

        /// <summary>
        /// Asserts that an IntranetBusinessException is valid.
        /// </summary>
        /// <param name="intranetBusinessException">The IntranetBusinessException to assert on.</param>
        /// <param name="expectedExceptionMessage">The expected exception message.</param>
        /// <param name="expectedArguments">The expected arguments for the exception message.</param>
        public static void AssertIntranetBusinessExceptionIsValid(IntranetBusinessException intranetBusinessException, ExceptionMessage expectedExceptionMessage, params object[] expectedArguments)
        {
            Assert.That(intranetBusinessException, Is.Not.Null);
            Assert.That(intranetBusinessException.Message, Is.Not.Null);
            Assert.That(intranetBusinessException.Message, Is.Not.Empty);
            Assert.That(intranetBusinessException.Message, Is.EqualTo(Resource.GetExceptionMessage(expectedExceptionMessage, expectedArguments)));
            Assert.That(intranetBusinessException.InnerException, Is.Null);
        }

        /// <summary>
        /// Asserts that an IntranetRepositoryException is valid.
        /// </summary>
        /// <param name="intranetRepositoryException">The IntranetRepositoryException to assert on.</param>
        /// <param name="expectedExceptionMessage">The expected exception message.</param>
        /// <param name="expectedArguments">The expected arguments for the exception message.</param>
        public static void AssertIntranetRepositoryExceptionIsValid(IntranetRepositoryException intranetRepositoryException, ExceptionMessage expectedExceptionMessage, params object [] expectedArguments)
        {
            Assert.That(intranetRepositoryException, Is.Not.Null);
            Assert.That(intranetRepositoryException.Message, Is.Not.Null);
            Assert.That(intranetRepositoryException.Message, Is.Not.Empty);
            Assert.That(intranetRepositoryException.Message, Is.EqualTo(Resource.GetExceptionMessage(expectedExceptionMessage, expectedArguments)));
            Assert.That(intranetRepositoryException.InnerException, Is.Null);
        }

        /// <summary>
        /// Asserts that an IntranetSystemException is valid.
        /// </summary>
        /// <param name="intranetSystemException">The IntranetSystemException to assert on.</param>
        /// <param name="expectedExceptionMessage">The expected exception message.</param>
        /// <param name="expectedArguments">The expected arguments for the exception message.</param>
        public static void AssertIntranetSystemExceptionIsValid(IntranetSystemException intranetSystemException, ExceptionMessage expectedExceptionMessage, params object[] expectedArguments)
        {
            Assert.That(intranetSystemException, Is.Not.Null);
            Assert.That(intranetSystemException.Message, Is.Not.Null);
            Assert.That(intranetSystemException.Message, Is.Not.Empty);
            Assert.That(intranetSystemException.Message, Is.EqualTo(Resource.GetExceptionMessage(expectedExceptionMessage, expectedArguments)));
            Assert.That(intranetSystemException.InnerException, Is.Null);
        }
    }
}
