using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tester ObjectMapper.
    /// </summary>
    [TestFixture]
    public class ObjectMapperTests
    {
        /// <summary>
        /// Tester, at ObjectMapper kan initieres.
        /// </summary>
        [Test]
        public void TestAtObjectMapperKanInitieres()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at et regnskab kan mappes til et regnskabslisteview.
        /// </summary>
        [Test]
        public void TestAtRegnskabKanMappesTilRegnskabslisteView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var regnskabslisteView = objectMapper.Map<Regnskab, RegnskabslisteView>(regnskab);
            Assert.That(regnskabslisteView, Is.Not.Null);
            Assert.That(regnskabslisteView.Number, Is.EqualTo(1));
            Assert.That(regnskabslisteView.Navn, Is.Not.Null);
            Assert.That(regnskabslisteView.Navn, Is.EqualTo("Privatregnskab, Ole Sørensen"));
        }
    }
}
