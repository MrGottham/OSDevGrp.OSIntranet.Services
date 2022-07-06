using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain
{
    /// <summary>
    /// Tester hjælper til en liste af domæneobjekter.
    /// </summary>
    [TestFixture]
    public class DomainObjectListHelperTests
    {
        /// <summary>
        /// Egen klasse til test af hjælper på en liste af domæneobjekter.
        /// </summary>
        private class MyDomainObjectListHelper : DomainObjectListHelper<OSIntranet.Domain.Fælles.System, int>
        {
            /// <summary>
            /// Danner egen klasse til test af hjælper på en liste af domæneobjekter.
            /// </summary>
            /// <param name="systemer">Systemer.</param>
            public MyDomainObjectListHelper(IEnumerable<OSIntranet.Domain.Fælles.System> systemer)
                : base(systemer)
            {
            }

            /// <summary>
            /// Henter og returnerer er givent regnskab.
            /// </summary>
            /// <param name="id">Unik identifikation af regnskabet.</param>
            /// <returns></returns>
            public override OSIntranet.Domain.Fælles.System GetById(int id)
            {
                return DomainObjetcs.Single(m => m.Nummer == id);
            }
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis listen af domæneobjekter er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDomainObjectsErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new MyDomainObjectListHelper(null));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Tester, at GetById henter et givent domæneobjekt.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterDomainObject()
        {
            var fixture = new Fixture();
            var systemer = fixture.CreateMany<OSIntranet.Domain.Fælles.System>(3).ToList();
            var domainObjectListHelper = new MyDomainObjectListHelper(systemer);
            Assert.That(domainObjectListHelper, Is.Not.Null);

            var system = domainObjectListHelper.GetById(systemer.ElementAt(1).Nummer);
            Assert.That(system, Is.Not.Null);
            Assert.That(system.Nummer, Is.EqualTo(systemer.ElementAt(1).Nummer));
            Assert.That(system.Titel, Is.Not.Null);
            Assert.That(system.Titel, Is.EqualTo(systemer.ElementAt(1).Titel));
            Assert.That(system.Properties, Is.EqualTo(systemer.ElementAt(1).Properties));
            Assert.That(system.Kalender, Is.EqualTo(systemer.ElementAt(1).Kalender));
        }
    }
}