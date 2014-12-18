using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Kalender;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Kalender
{
    /// <summary>
    /// Tester domæneobjekt for en kalenderbruger.
    /// </summary>
    [TestFixture]
    public class BrugerTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kalenderbruger.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBruger()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var system = fixture.Create<ISystem>();
            var id = fixture.Create<int>();
            var initiaer = fixture.Create<string>();
            var navn = fixture.Create<string>();
            var bruger = new Bruger(system, id, initiaer, navn);
            Assert.That(bruger, Is.Not.Null);
            Assert.That(bruger.System, Is.Not.Null);
            Assert.That(bruger.System, Is.EqualTo(system));
            Assert.That(bruger.Id, Is.EqualTo(id));
            Assert.That(bruger.Initialer, Is.Not.Null);
            Assert.That(bruger.Initialer, Is.EqualTo(initiaer));
            Assert.That(bruger.Navn, Is.Not.Null);
            Assert.That(bruger.Navn, Is.EqualTo(navn));
            Assert.That(bruger.UserName, Is.Null);
        }
        
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis systemet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisSystemErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<ISystem>(null);

            Assert.Throws<ArgumentNullException>(
                () =>
                new Bruger(fixture.Create<ISystem>(), fixture.Create<int>(),
                           fixture.Create<string>(), fixture.Create<string>()));

        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis initialer er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisInitialerErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            Assert.Throws<ArgumentNullException>(
                () =>
                new Bruger(fixture.Create<ISystem>(), fixture.Create<int>(), null,
                           fixture.Create<string>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis initialer er tom.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisInitialerErEmpty()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            Assert.Throws<ArgumentNullException>(
                () =>
                new Bruger(fixture.Create<ISystem>(), fixture.Create<int>(), string.Empty,
                           fixture.Create<string>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis navnet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisNavnErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            Assert.Throws<ArgumentNullException>(
                () =>
                new Bruger(fixture.Create<ISystem>(), fixture.Create<int>(),
                           fixture.Create<string>(), null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisNavnErEmpty()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            Assert.Throws<ArgumentNullException>(
                () =>
                new Bruger(fixture.Create<ISystem>(), fixture.Create<int>(),
                           fixture.Create<string>(), string.Empty));
        }

        /// <summary>
        /// Tester, at Initialer kan ændres.
        /// </summary>
        [Test]
        public void TestAtInitialerÆndres()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var bruger = fixture.Create<Bruger>();
            Assert.That(bruger, Is.Not.Null);

            var initialer = fixture.Create<string>();
            bruger.Initialer = initialer;
            Assert.That(bruger.Initialer, Is.Not.Null);
            Assert.That(bruger.Initialer, Is.EqualTo(initialer));
        }

        /// <summary>
        /// Tester, at Initialer kaster en ArgumentNullException, hvis værdien er null.
        /// </summary>
        [Test]
        public void TestAtInitialerKasterArgumentNullExceptionHvisValueErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var bruger = fixture.Create<Bruger>();
            Assert.That(bruger, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => bruger.Initialer = null);
        }

        /// <summary>
        /// Tester, at Initialer kaster en ArgumentNullException, hvis værdien er tom.
        /// </summary>
        [Test]
        public void TestAtInitialerKasterArgumentNullExceptionHvisValueErEmpty()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var bruger = fixture.Create<Bruger>();
            Assert.That(bruger, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => bruger.Initialer = string.Empty);
        }

        /// <summary>
        /// Tester, at Navn kan ændres.
        /// </summary>
        [Test]
        public void TestAtNavnÆndres()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var bruger = fixture.Create<Bruger>();
            Assert.That(bruger, Is.Not.Null);

            var navn = fixture.Create<string>();
            bruger.Navn = navn;
            Assert.That(bruger.Navn, Is.Not.Null);
            Assert.That(bruger.Navn, Is.EqualTo(navn));
        }

        /// <summary>
        /// Tester, at Navn kaster en ArgumentNullException, hvis værdien er null.
        /// </summary>
        [Test]
        public void TestAtNavnKasterArgumentNullExceptionHvisValueErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var bruger = fixture.Create<Bruger>();
            Assert.That(bruger, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => bruger.Navn = null);
        }

        /// <summary>
        /// Tester, at Navn kaster en ArgumentNullException, hvis værdien er tom.
        /// </summary>
        [Test]
        public void TestAtNavnKasterArgumentNullExceptionHvisValueErEmpty()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var bruger = fixture.Create<Bruger>();
            Assert.That(bruger, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => bruger.Navn = string.Empty);
        }

        /// <summary>
        /// Tester, at UserName kan ændres.
        /// </summary>
        [Test]
        public void TestAtUserNameÆndres()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var bruger = fixture.Create<Bruger>();
            Assert.That(bruger, Is.Not.Null);

            var userName = fixture.Create<string>();
            bruger.UserName = userName;
            Assert.That(bruger.UserName, Is.Not.Null);
            Assert.That(bruger.UserName, Is.EqualTo(userName));
        }
    }
}
