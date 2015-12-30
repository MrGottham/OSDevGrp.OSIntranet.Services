using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Flows
{
    /// <summary>
    /// Integration tests which tests flows with a household member.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class HouseholdMemberTests
    {
        #region Private variables

        private ILogicExecutor _logicExecutor;
        private IHouseholdDataRepository _householdDataRepository;
        private IFoodWasteHouseholdService _householdService;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _logicExecutor = container.Resolve<ILogicExecutor>();
            _householdDataRepository = container.Resolve<IHouseholdDataRepository>();
            _householdService = container.Resolve<IFoodWasteHouseholdService>();
        }

        /// <summary>
        /// Tests the flow for creation of a household member.
        /// </summary>
        [Test]
        public void TestHouseholdMemberCreationFlow()
        {
            using (var executor = new FlowTestExecutor())
            {
                var translationInfoCollection = _householdService.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                var translationInfoIdentifier = translationInfoCollection.Take(1).First().TranslationInfoIdentifier;
                var householdMemberIdentifier = _logicExecutor.HouseholdMemberAdd(executor.MailAddress, translationInfoIdentifier);
                try
                {
                }
                finally
                {
                    _householdDataRepository.Delete(_householdDataRepository.Get<IHouseholdMember>(householdMemberIdentifier));
                }
            }
        }
    }
}
