using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tests the logic executor which can execute basic logic.
    /// </summary>
    [TestFixture]
    public class LogicExecuterTests
    {
        /// <summary>
        /// Tests that the constructor initialize the logic executor which can execute basic logic.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeLogicExecutor()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var logicExecutor = new LogicExecuter(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the command bus is null..
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCommandBusIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new LogicExecuter(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("commandBus"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
