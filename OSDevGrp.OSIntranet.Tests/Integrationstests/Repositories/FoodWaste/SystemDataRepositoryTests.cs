using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories.FoodWaste
{
    /// <summary>
    /// Integration tests for the repository which can access system data for the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class SystemDataRepositoryTests
    {
        #region Private variables

        private ISystemDataRepository _systemDataRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _systemDataRepository = container.Resolve<ISystemDataRepository>();
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet gets translations for a given identifiable domain object.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetGetsTranslationsForIdentifiableDomainObject()
        {
            var translationInfos = _systemDataRepository.TranslationInfoGetAll().ToArray();
            var tranlationOf = new Translation(Guid.NewGuid(), translationInfos.FirstOrDefault(), "Test")
            {
                Identifier = Guid.NewGuid()
            };
            // ReSharper disable PossibleInvalidOperationException
            var translation = _systemDataRepository.Insert(new Translation(tranlationOf.Identifier.Value, translationInfos.FirstOrDefault(), "Test"));
            // ReSharper restore PossibleInvalidOperationException
            try
            {
                // ReSharper disable PossibleInvalidOperationException
                var result = _systemDataRepository.TranslationsForDomainObjectGet(tranlationOf);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.Not.Empty);
            }
            finally
            {
                _systemDataRepository.Delete(translation);
            }
        }

        /// <summary>
        /// Tests that Get gets a given translation
        /// </summary>
        [Test]
        public void TestThatGetGetsTranslation()
        {
            var translationInfos = _systemDataRepository.TranslationInfoGetAll();
            var translation = _systemDataRepository.Insert(new Translation(Guid.NewGuid(), translationInfos.FirstOrDefault(), "Test"));
            try
            {
                // ReSharper disable PossibleInvalidOperationException
                var result = _systemDataRepository.Get<ITranslation>(translation.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Identifier, Is.EqualTo(translation.Identifier));
            }
            finally
            {
                _systemDataRepository.Delete(translation);
            }
        }

        /// <summary>
        /// Tests that Update updates a given translation
        /// </summary>
        [Test]
        public void TestThatUpdateUpdatesTranslation()
        {
            var translationInfos = _systemDataRepository.TranslationInfoGetAll();
            var translation = _systemDataRepository.Insert(new Translation(Guid.NewGuid(), translationInfos.FirstOrDefault(), "Test"));
            try
            {
                translation.Value = "Testing";
                _systemDataRepository.Update(translation);

                // ReSharper disable PossibleInvalidOperationException
                var result = _systemDataRepository.Get<ITranslation>(translation.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Value, Is.Not.Empty);
                Assert.That(result.Value, Is.EqualTo(translation.Value));
            }
            finally
            {
                _systemDataRepository.Delete(translation);
            }
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll returns all the translation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllReturnsTranslationInfos()
        {
            var translationInfos = _systemDataRepository.TranslationInfoGetAll();
            Assert.That(translationInfos, Is.Not.Null);
            Assert.That(translationInfos, Is.Not.Empty);
            Assert.That(translationInfos.Count(), Is.EqualTo(2));
        }
    }
}
