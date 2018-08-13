using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tests the Argument Null Guard.
    /// </summary>
    [TestFixture]
    public class ArgumentNullGuardTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tests that NotNull throws an ArgumentNullException when the name of the argument is null, empty or white space.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatNotNullThrowsArgumentNullExceptionWhenArgumentNameIsNullEmptyOrWhiteSpace(string argumentName)
        {
            IArgumentNullGuard sut = CreateSut();

            object value = _fixture.Create<object>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNull(value, argumentName));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "argumentName");
        }

        /// <summary>
        /// Tests that NotNull throws an ArgumentNullException when the object value is null.
        /// </summary>
        [Test]
        public void TestThatNotNullThrowsArgumentNullExceptionWhenObjectValueIsNull()
        {
            IArgumentNullGuard sut = CreateSut();

            const object value = null;
            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNull(value, argumentName));

            TestHelper.AssertArgumentNullExceptionIsValid(result, argumentName);
        }

        /// <summary>
        /// Tests that NotNull returns the instance of Argument Null Guard when the object value is not null.
        /// </summary>
        [Test]
        public void TestThatNotNullReturnsArgumentNullGuardWhenObjectValueIsNotNull()
        {
            IArgumentNullGuard sut = CreateSut();

            object value = _fixture.Create<object>();
            string argumentName = _fixture.Create<string>();
            IArgumentNullGuard result = sut.NotNull(value, argumentName);

            Assert.IsNotNull(result);
            Assert.AreSame(sut, result);
        }

        /// <summary>
        /// Tests that NotNull returns the instance of Argument Null Guard when the string value is not null.
        /// </summary>
        [Test]
        public void TestThatNotNullReturnsArgumentNullGuardWhenStringValueIsNotNull()
        {
            IArgumentNullGuard sut = CreateSut();

            string value = _fixture.Create<string>();
            string argumentName = _fixture.Create<string>();
            IArgumentNullGuard result = sut.NotNull(value, argumentName);

            Assert.IsNotNull(result);
            Assert.AreSame(sut, result);
        }

        /// <summary>
        /// Tests that NotNull returns the instance of Argument Null Guard when the string value is empty or white space.
        /// </summary>
        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatNotNullReturnsArgumentNullGuardWhenStringValueIsEmptyOrWhitespace(string value)
        {
            IArgumentNullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            IArgumentNullGuard result = sut.NotNull(value, argumentName);

            Assert.IsNotNull(result);
            Assert.AreSame(sut, result);
        }

        /// <summary>
        /// Tests that NotNull throws an ArgumentNullException when the string value is null.
        /// </summary>
        [Test]
        public void TestThatNotNullThrowsArgumentNullExceptionWhenStringValueIsNull()
        {
            IArgumentNullGuard sut = CreateSut();

            const string value = null;
            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNull(value, argumentName));

            TestHelper.AssertArgumentNullExceptionIsValid(result, argumentName);
        }

        /// <summary>
        /// Tests that NotNullOrEmpty throws an ArgumentNullException when the name of the argument is null, empty or white space.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatNotNullOrEmptyThrowsArgumentNullExceptionWhenArgumentNameIsNullEmptyOrWhiteSpace(string argumentName)
        {
            IArgumentNullGuard sut = CreateSut();

            string value = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrEmpty(value, argumentName));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "argumentName");
        }

        /// <summary>
        /// Tests that NotNullOrEmpty throws an ArgumentNullException when the string value is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatNotNullOrEmptyThrowsArgumentNullExceptionWhenStringValueIsNullOrEmpty(string value)
        {
            IArgumentNullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrEmpty(value, argumentName));

            TestHelper.AssertArgumentNullExceptionIsValid(result, argumentName);
        }

        /// <summary>
        /// Tests that NotNullOrEmpty returns the instance of Argument Null Guard when the string value is not null.
        /// </summary>
        [Test]
        public void TestThatNotNullOrEmptyReturnsArgumentNullGuardWhenStringValueIsNotNull()
        {
            IArgumentNullGuard sut = CreateSut();

            string value = _fixture.Create<string>();
            string argumentName = _fixture.Create<string>();
            IArgumentNullGuard result = sut.NotNullOrEmpty(value, argumentName);

            Assert.IsNotNull(result);
            Assert.AreSame(sut, result);
        }

        /// <summary>
        /// Tests that NotNullOrEmpty returns the instance of Argument Null Guard when the string value is white spae.
        /// </summary>
        [Test]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatNotNullOrEmptyReturnsArgumentNullGuardWhenStringValueIsNotNull(string value)
        {
            IArgumentNullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            IArgumentNullGuard result = sut.NotNullOrEmpty(value, argumentName);

            Assert.IsNotNull(result);
            Assert.AreSame(sut, result);
        }

        /// <summary>
        /// Tests that NotNullOrWhiteSpace throws an ArgumentNullException when the name of the argument is null, empty or white space.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatNotNullOrWhiteSpaceThrowsArgumentNullExceptionWhenArgumentNameIsNullEmptyOrWhiteSpace(string argumentName)
        {
            IArgumentNullGuard sut = CreateSut();

            string value = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrWhiteSpace(value, argumentName));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "argumentName");
        }

        /// <summary>
        /// Tests that NotNullOrWhiteSpace throws an ArgumentNullException when the string value is null, empty or white spacce.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatNotNullOrWhiteSpaceThrowsArgumentNullExceptionWhenStringValueIsNullOrWhiteSpace(string value)
        {
            IArgumentNullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrWhiteSpace(value, argumentName));

            TestHelper.AssertArgumentNullExceptionIsValid(result, argumentName);
        }

        /// <summary>
        /// Tests that NotNullOrWhiteSpace returns the instance of Argument Null Guard when the string value is not null.
        /// </summary>
        [Test]
        public void TestThatNotNullOrWhiteSpaceReturnsArgumentNullGuardWhenStringValueIsNotNull()
        {
            IArgumentNullGuard sut = CreateSut();

            string value = _fixture.Create<string>();
            string argumentName = _fixture.Create<string>();
            IArgumentNullGuard result = sut.NotNullOrWhiteSpace(value, argumentName);

            Assert.IsNotNull(result);
            Assert.AreSame(sut, result);
        }

        /// <summary>
        /// Creates an instance of the Argument Null Guard for unit testing.
        /// </summary>
        /// <returns>Instance of the Argument Null Guard for unit testing.</returns>
        private IArgumentNullGuard CreateSut()
        {
            return new ArgumentNullGuard();
        }
    }
}
