using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Kalender
{
    /// <summary>
    /// Tester hjælper til en liste af aftaler i et givent system.
    /// </summary>
    [TestFixture]
    public class AftalelisteHelperTests
    {
        /// <summary>
        /// Tester, at GetById henter og returnerer en given aftale.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterAftale()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            var aftaler = new List<IAftale>
                              {
                                  new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                             fixture.Create<DateTime>(),
                                             fixture.Create<DateTime>().AddMinutes(15),
                                             fixture.Create<string>()),
                                  new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                             fixture.Create<DateTime>(),
                                             fixture.Create<DateTime>().AddMinutes(15),
                                             fixture.Create<string>()),
                                  new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                             fixture.Create<DateTime>(),
                                             fixture.Create<DateTime>().AddMinutes(15),
                                             fixture.Create<string>())
                              };
            var aftalelisteHelper = new AftalelisteHelper(aftaler);
            Assert.That(aftalelisteHelper, Is.Not.Null);

            var aftale = aftalelisteHelper.GetById(aftaler.ElementAt(1).Id);
            Assert.That(aftale, Is.Not.Null);
            Assert.That(aftale.Id, Is.EqualTo(aftaler.ElementAt(1).Id));
            Assert.That(aftale.Emne, Is.Not.Null);
            Assert.That(aftale.Emne, Is.EqualTo(aftaler.ElementAt(1).Emne));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis aftalen ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisAftaleIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            var aftaler = new List<IAftale>
                              {
                                  new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                             fixture.Create<DateTime>(),
                                             fixture.Create<DateTime>().AddMinutes(15),
                                             fixture.Create<string>()),
                                  new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                             fixture.Create<DateTime>(),
                                             fixture.Create<DateTime>().AddMinutes(15),
                                             fixture.Create<string>()),
                                  new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                             fixture.Create<DateTime>(),
                                             fixture.Create<DateTime>().AddMinutes(15),
                                             fixture.Create<string>())
                              };
            var aftalelisteHelper = new AftalelisteHelper(aftaler);
            Assert.That(aftalelisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => aftalelisteHelper.GetById(-1));
        }
    }
}
