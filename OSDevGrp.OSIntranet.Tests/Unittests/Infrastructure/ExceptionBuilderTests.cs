using System;
using System.Reflection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Infrastructure;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tests the Exception builder.
    /// </summary>
    [TestFixture]
    public class ExceptionBuilderTests
    {
        /// <summary>
        /// Private class for testing exception from command handlers.
        /// </summary>
        private class MyCommandHandlerWithoutReturnValue : CommandHandlerNonTransactionalBase, ICommandHandler<ICommand>
        {
            #region Private variables

            private readonly IExceptionBuilder _exceptionBuilder = new ExceptionBuilder();

            #endregion

            #region Methods

            /// <summary>
            /// Executes the functionality for the command.
            /// </summary>
            /// <param name="command">Command for to execute functionality.</param>
            public void Execute(ICommand command)
            {
            }

            /// <summary>
            /// Handle an exception that occurred when executing functionality for the command.
            /// </summary>
            /// <param name="command">Command for to execute functionality.</param>
            /// <param name="exception">Exception that occurred when executing functionality for the command.</param>
            public void HandleException(ICommand command, Exception exception)
            {
                throw _exceptionBuilder.Build(exception, MethodBase.GetCurrentMethod());
            }

            #endregion
        }

        /// <summary>
        /// Private class for testing exception from command handlers.
        /// </summary>
        private class MyCommandHandlerWithReturnValue : CommandHandlerNonTransactionalBase, ICommandHandler<ICommand, Guid>
        {
            #region Private variables

            private readonly IExceptionBuilder _exceptionBuilder = new ExceptionBuilder();

            #endregion

            #region Methods

            /// <summary>
            /// Executes the functionality for the command.
            /// </summary>
            /// <param name="command">Command for to execute functionality.</param>
            public Guid Execute(ICommand command)
            {
                return Guid.Empty;
            }

            /// <summary>
            /// Handle an exception that occurred when executing functionality for the command.
            /// </summary>
            /// <param name="command">Command for to execute functionality.</param>
            /// <param name="exception">Exception that occurred when executing functionality for the command.</param>
            public void HandleException(ICommand command, Exception exception)
            {
                throw _exceptionBuilder.Build(exception, MethodBase.GetCurrentMethod());
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize the exception builder.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeExceptionBuilder()
        {
            var exceptionBuilder = new ExceptionBuilder();
            Assert.That(exceptionBuilder, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Build throws an ArgumentNullException when the exception is null.
        /// </summary>
        [Test]
        public void TestThatBuildThrowsArgumentNullExceptionWhenExceptionIsNull()
        {
            var exceptionBuilder = new ExceptionBuilder();
            Assert.That(exceptionBuilder, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => exceptionBuilder.Build(null, MethodBase.GetCurrentMethod()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exception"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Build throws an ArgumentNullException when the method where the exception occurred is null.
        /// </summary>
        [Test]
        public void TestThatBuildThrowsArgumentNullExceptionWhenMethodIsNull()
        {
            var fixture = new Fixture();

            var exceptionBuilder = new ExceptionBuilder();
            Assert.That(exceptionBuilder, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => exceptionBuilder.Build(fixture.Create<Exception>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("method"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Build returns the IntranetRepositoryException when the exception is a IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestThatBuildReturnsIntranetRepositoryExceptionWhenExceptionIsIntranetRepositoryException()
        {
            var fixture = new Fixture();
            var intranetRepositoryException = fixture.Create<IntranetRepositoryException>();

            var exceptionBuilder = new ExceptionBuilder();
            Assert.That(exceptionBuilder, Is.Not.Null);

            var result = exceptionBuilder.Build(intranetRepositoryException, MethodBase.GetCurrentMethod());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<IntranetRepositoryException>());
            Assert.That(result, Is.EqualTo(intranetRepositoryException));
        }

        /// <summary>
        /// Tests that Build returns the IntranetBusinessException when the exception is a IntranetBusinessException.
        /// </summary>
        [Test]
        public void TestThatBuildReturnsIntranetBusinessExceptionWhenExceptionIsIntranetBusinessException()
        {
            var fixture = new Fixture();
            var intranetBusinessException = fixture.Create<IntranetBusinessException>();

            var exceptionBuilder = new ExceptionBuilder();
            Assert.That(exceptionBuilder, Is.Not.Null);

            var result = exceptionBuilder.Build(intranetBusinessException, MethodBase.GetCurrentMethod());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<IntranetBusinessException>());
            Assert.That(result, Is.EqualTo(intranetBusinessException));
        }

        /// <summary>
        /// Tests that Build returns the IntranetSystemException when the exception is a IntranetSystemException.
        /// </summary>
        [Test]
        public void TestThatBuildReturnsIntranetSystemExceptionWhenExceptionIsIntranetSystemException()
        {
            var fixture = new Fixture();
            var intranetSystemException = fixture.Create<IntranetSystemException>();

            var exceptionBuilder = new ExceptionBuilder();
            Assert.That(exceptionBuilder, Is.Not.Null);

            var result = exceptionBuilder.Build(intranetSystemException, MethodBase.GetCurrentMethod());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<IntranetSystemException>());
            Assert.That(result, Is.EqualTo(intranetSystemException));
        }

        /// <summary>
        /// Tests that Build returns an IntranetSystemException when an exception occurs while executing a command on a command handler without a return value.
        /// </summary>
        [Test]
        public void TestThatBuildReturnsIntranetSystemExceptionWhenExceptionOccursWhileExecutingCommandHandlerWithoutReturnValue()
        {
            var fixture = new Fixture();
            var exception = fixture.Create<Exception>();

            var myCommandHandlerWithoutReturnValue = new MyCommandHandlerWithoutReturnValue();
            Assert.That(myCommandHandlerWithoutReturnValue, Is.Not.Null);

            var result = Assert.Throws<IntranetSystemException>(() => myCommandHandlerWithoutReturnValue.HandleException(MockRepository.GenerateMock<ICommand>(), exception));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
            Assert.That(result.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithoutReturnValue, typeof (ICommand).Name, exception.Message)));
            Assert.That(result.InnerException, Is.Not.Null);
            Assert.That(result.InnerException, Is.EqualTo(exception));
        }

        /// <summary>
        /// Tests that Build returns an IntranetSystemException when an exception occurs while executing a command on a command handler with a return value.
        /// </summary>
        [Test]
        public void TestThatBuildReturnsIntranetSystemExceptionWhenExceptionOccursWhileExecutingCommandHandlerWithReturnValue()
        {
            var fixture = new Fixture();
            var exception = fixture.Create<Exception>();

            var myCommandHandlerWithReturnValue = new MyCommandHandlerWithReturnValue();
            Assert.That(myCommandHandlerWithReturnValue, Is.Not.Null);

            var result = Assert.Throws<IntranetSystemException>(() => myCommandHandlerWithReturnValue.HandleException(MockRepository.GenerateMock<ICommand>(), exception));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
            Assert.That(result.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithoutReturnValue, typeof (ICommand).Name, typeof (Guid).Name, exception.Message)));
            Assert.That(result.InnerException, Is.Not.Null);
            Assert.That(result.InnerException, Is.EqualTo(exception));
        }
    }
}
