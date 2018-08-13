using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Validation;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure.Validation
{
    /// <summary>
    /// Tests the specification which encapsulates validation rules.
    /// </summary>
    [TestFixture]
    public class SpecificationTests
    {
        /// <summary>
        /// Tests that the constructor initialize a specification.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeSpecification()
        {
            var specification = new Specification();
            Assert.That(specification, Is.Not.Null);
        }

        /// <summary>
        /// Tests that IsSatisfiedBy throws an ArgumentNullException when the delegate for validation which returns a boolean is null.
        /// </summary>
        [Test]
        public void TestThatIsSatisfiedByThrowsArgumentNullExceptionWhenCandidateIsNull()
        {
            var fixture = new Fixture();

            var specification = new Specification();
            Assert.That(specification, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => specification.IsSatisfiedBy(null, fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("candidate"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsSatisfiedBy throws an ArgumentNullException when the exception which should be thrown when the delegate returns false is null.
        /// </summary>
        [Test]
        public void TestThatIsSatisfiedByThrowsArgumentNullExceptionWhenExceptionIsNull()
        {
            var specification = new Specification();
            Assert.That(specification, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => specification.IsSatisfiedBy(() => true, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exception"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsSatisfiedBy returns the specification on which it was added.
        /// </summary>
        [Test]
        public void TestThatIsSatisfiedByReturnsSpecification()
        {
            var fixture = new Fixture();

            var specification = new Specification();
            Assert.That(specification, Is.Not.Null);

            var result = specification.IsSatisfiedBy(() => true, fixture.Create<Exception>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(specification));
        }

        /// <summary>
        /// Tests that Evaluate does not throw an Exception when all candidates returns true.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotThrowExceptionWhenAllCandidatesReturnsTrue()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var specification = new Specification();
            Assert.That(specification, Is.Not.Null);

            for (var i = 0; i < random.Next(5, 10); i++)
            {
                specification.IsSatisfiedBy(() => true, fixture.Create<Exception>());
            }
            specification.Evaluate();
        }

        /// <summary>
        /// Tests that Evaluate throws an Exception when a candidate returns false.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowExceptionWhenOneCandidatesReturnsFalse()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var specification = new Specification();
            Assert.That(specification, Is.Not.Null);

            for (var i = 0; i < random.Next(5, 10); i++)
            {
                specification.IsSatisfiedBy(() => true, fixture.Create<Exception>());
            }
            var exceptionToThrow = fixture.Create<Exception>();
            specification.IsSatisfiedBy(() => false, exceptionToThrow);

            var exception = Assert.Throws<Exception>(specification.Evaluate);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
