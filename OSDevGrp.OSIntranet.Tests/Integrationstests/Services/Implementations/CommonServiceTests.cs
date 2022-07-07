using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.Implementations
{
    /// <summary>
    /// Integrationstester servicen til fælles elementer.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class CommonServiceTests
    {
        #region Private variables

        private ICommonService _service;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _service = container.Resolve<ICommonService>();
        }

        /// <summary>
        /// Tester, at systemer under OSWEBDB hentes.
        /// </summary>
        [Test]
        public void TestAtSystemerHentes()
        {
            var query = new SystemerGetQuery();
            var result = _service.SystemerGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }
    }
}
