using System;
using System.Reflection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Infrastructure;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tests the builder which can a FaultException with FaultDetails which food waste services can throw.
    /// </summary>
    [TestFixture]
    public class FoodWasteFaultExceptionBuilderTests
    {
        /// <summary>
        /// Tests that Build throws an ArgumentNullException when the exception which the FaultException should be based on is null.
        /// </summary>
        [Test]
        public void TestThatBuildThrowsArgumentNullExceptionIfExceptionIsNull()
        {
            var fixture = new Fixture();

            var foodWasteFaultExceptionBuilder = new FoodWasteFaultExceptionBuilder();
            Assert.That(foodWasteFaultExceptionBuilder, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteFaultExceptionBuilder.Build(null, fixture.Create<string>(), MethodBase.GetCurrentMethod()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exception"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Build throws an ArgumentNullException when service name is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatBuildThrowsArgumentNullExceptionIfServiceNameIsInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var foodWasteFaultExceptionBuilder = new FoodWasteFaultExceptionBuilder();
            Assert.That(foodWasteFaultExceptionBuilder, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteFaultExceptionBuilder.Build(fixture.Create<Exception>(), invalidValue, MethodBase.GetCurrentMethod()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("serviceName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Build throws an ArgumentNullException when method informations is null.
        /// </summary>
        [Test]
        public void TestThatBuildThrowsArgumentNullExceptionIfMethodInfoIsNull()
        {
            var fixture = new Fixture();

            var foodWasteFaultExceptionBuilder = new FoodWasteFaultExceptionBuilder();
            Assert.That(foodWasteFaultExceptionBuilder, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteFaultExceptionBuilder.Build(fixture.Create<Exception>(), fixture.Create<string>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("methodInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Build build a FaultException with FaultDetails for an IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestThatBuildBuildsFaultExceptionWithFaultDetailsForIntranetRepositoryException()
        {
            var fixture = new Fixture();
            var exception = fixture.Create<IntranetRepositoryException>();
            var serviceName = fixture.Create<string>();
            var methodInfo = MethodBase.GetCurrentMethod();

            var foodWasteFaultExceptionBuilder = new FoodWasteFaultExceptionBuilder();
            Assert.That(foodWasteFaultExceptionBuilder, Is.Not.Null);

            var faultException = foodWasteFaultExceptionBuilder.Build(exception, serviceName, methodInfo);
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Reason, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);
            Assert.That(faultException.Detail.FaultType, Is.EqualTo(FoodWasteFaultType.RepositoryFault));
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
            Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(exception.Message));
            Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceName, Is.EqualTo(serviceName));
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo(methodInfo.Name));
            Assert.That(faultException.Detail.StackTrace, Is.EqualTo(exception.StackTrace));
        }

        /// <summary>
        /// Tests that Build build a FaultException with FaultDetails for an IntranetSystemException.
        /// </summary>
        [Test]
        public void TestThatBuildBuildsFaultExceptionWithFaultDetailsForIntranetSystemException()
        {
            var fixture = new Fixture();
            var exception = fixture.Create<IntranetSystemException>();
            var serviceName = fixture.Create<string>();
            var methodInfo = MethodBase.GetCurrentMethod();

            var foodWasteFaultExceptionBuilder = new FoodWasteFaultExceptionBuilder();
            Assert.That(foodWasteFaultExceptionBuilder, Is.Not.Null);

            var faultException = foodWasteFaultExceptionBuilder.Build(exception, serviceName, methodInfo);
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Reason, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);
            Assert.That(faultException.Detail.FaultType, Is.EqualTo(FoodWasteFaultType.SystemFault));
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
            Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(exception.Message));
            Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceName, Is.EqualTo(serviceName));
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo(methodInfo.Name));
            Assert.That(faultException.Detail.StackTrace, Is.EqualTo(exception.StackTrace));
        }

        /// <summary>
        /// Tests that Build build a FaultException with FaultDetails for an IntranetBusinessException.
        /// </summary>
        [Test]
        public void TestThatBuildBuildsFaultExceptionWithFaultDetailsForIntranetBusinessException()
        {
            var fixture = new Fixture();
            var exception = fixture.Create<IntranetBusinessException>();
            var serviceName = fixture.Create<string>();
            var methodInfo = MethodBase.GetCurrentMethod();

            var foodWasteFaultExceptionBuilder = new FoodWasteFaultExceptionBuilder();
            Assert.That(foodWasteFaultExceptionBuilder, Is.Not.Null);

            var faultException = foodWasteFaultExceptionBuilder.Build(exception, serviceName, methodInfo);
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Reason, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);
            Assert.That(faultException.Detail.FaultType, Is.EqualTo(FoodWasteFaultType.BusinessFault));
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
            Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(exception.Message));
            Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceName, Is.EqualTo(serviceName));
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo(methodInfo.Name));
            Assert.That(faultException.Detail.StackTrace, Is.EqualTo(exception.StackTrace));
        }

        /// <summary>
        /// Tests that Build build a FaultException with FaultDetails for an Exception.
        /// </summary>
        [Test]
        public void TestThatBuildBuildsFaultExceptionWithFaultDetailsForException()
        {
            var fixture = new Fixture();
            var exception = fixture.Create<Exception>();
            var serviceName = fixture.Create<string>();
            var methodInfo = MethodBase.GetCurrentMethod();

            var foodWasteFaultExceptionBuilder = new FoodWasteFaultExceptionBuilder();
            Assert.That(foodWasteFaultExceptionBuilder, Is.Not.Null);

            var faultException = foodWasteFaultExceptionBuilder.Build(exception, serviceName, methodInfo);
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Reason, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);
            Assert.That(faultException.Detail.FaultType, Is.EqualTo(FoodWasteFaultType.SystemFault));
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
            Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(exception.Message));
            Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceName, Is.EqualTo(serviceName));
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo(methodInfo.Name));
            Assert.That(faultException.Detail.StackTrace, Is.EqualTo(exception.StackTrace));
        }
    }
}
