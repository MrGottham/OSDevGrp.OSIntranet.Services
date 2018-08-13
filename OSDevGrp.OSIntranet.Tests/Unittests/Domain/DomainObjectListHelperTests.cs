using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
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
        private class MyDomainObjectListHelper : DomainObjectListHelper<Regnskab, int>
        {
            /// <summary>
            /// Danner egen klasse til test af hjælper på en liste af domæneobjekter.
            /// </summary>
            /// <param name="regnskaber">Regnskaber.</param>
            public MyDomainObjectListHelper(IEnumerable<Regnskab> regnskaber)
                : base(regnskaber)
            {
            }

            /// <summary>
            /// Henter og returnerer er givent regnskab.
            /// </summary>
            /// <param name="id">Unik identifikation af regnskabet.</param>
            /// <returns></returns>
            public override Regnskab GetById(int id)
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
            Assert.Throws<ArgumentNullException>(() => new MyDomainObjectListHelper(null));
        }

        /// <summary>
        /// Tester, at GetById henter et givent domæneobjekt.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterDomainObject()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var domainObjectListHelper = new MyDomainObjectListHelper(regnskaber);
            Assert.That(domainObjectListHelper, Is.Not.Null);

            var regnskab = domainObjectListHelper.GetById(regnskaber.ElementAt(1).Nummer);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(regnskaber.ElementAt(1).Nummer));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo(regnskaber.ElementAt(1).Navn));
        }
    }
}
