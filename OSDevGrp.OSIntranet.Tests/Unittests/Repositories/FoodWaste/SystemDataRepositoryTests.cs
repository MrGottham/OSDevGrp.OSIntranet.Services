using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.FoodWaste
{
    /// <summary>
    /// Tests the repository which can access system data for the food waste domain.
    /// </summary>
    [TestFixture]
    public class SystemDataRepositoryTests
    {
        /// <summary>
        /// Tests that the constructor initialize a repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeSystemDataRepository()
        {
            var foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProvider);
            Assert.That(systemDataRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the data provider which can access data in the food waste repository is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteDataProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new SystemDataRepository(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteDataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll calls GetCollection on FoodWasteDataProvider.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            var foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Anything))
                .Return(new List<TranslationInfoProxy>(0))
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProvider);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.TranslationInfoGetAll();

            foodWasteDataProvider.AssertWasCalled(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Equal("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos ORDER BY CultureName")));
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll returns the result from the data provider.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var translationInfoCollection = fixture.CreateMany<TranslationInfoProxy>(25).ToList();
            var foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Anything))
                .Return(translationInfoCollection)
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProvider);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.TranslationInfoGetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(translationInfoCollection));
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProvider);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.TranslationInfoGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProvider);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.TranslationInfoGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "TranslationInfoGetAll", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }
    }
}
