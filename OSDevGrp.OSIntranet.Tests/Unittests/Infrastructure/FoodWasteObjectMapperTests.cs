using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tests the object mapper which can map objects in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteObjectMapperTests
    {
        /// <summary>
        /// Tests that the object mapper which can map objects in the food waste domain can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodWasteObjectMapperCanBeInitialized()
        {
            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Map throws an ArgumentNullException if the source object to map is null.
        /// </summary>
        [Test]
        public void TestThatMapThrowsArgumentNullExceptionIfSourceIsNull()
        {
            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteObjectMapper.Map<object, object>(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("source"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Map throws an IntranetSystemException when the source object is identifiable and the identifier is null.
        /// </summary>
        [Test]
        public void TestThatMapThrowsIntranetSystemExceptionWhenSourceIsIsIdentifiableAndIdentifierIsNull()
        {
            var identifiableMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => foodWasteObjectMapper.Map<object, object>(identifiableMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiableMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Map throws an IntranetSystemException when the source object is identifiable and the identifier has no value.
        /// </summary>
        [Test]
        public void TestThatMapThrowsIntranetSystemExceptionWhenSourceIsIsIdentifiableAndIdentifierHasNoValue()
        {
            var identifiableMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => foodWasteObjectMapper.Map<object, object>(identifiableMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiableMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Map calls Translate on source if it's a translatable domain object and the translation culture is null.
        /// </summary>
        [Test]
        public void TestThatMapCallsTranslateOnSourceIfTranslatableAndTranslationCultureIsNull()
        {
            var translatableMock = DomainObjectMockBuilder.BuildFoodGroupMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<IFoodGroup, object>(translatableMock);

            translatableMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Map calls Translate on source if it's a translatable domain object and the translation culture is not null.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        public void TestThatMapCallsTranslateOnSourceIfTranslatableAndTranslationCultureIsNotNull(string cultureName)
        {
            var translatableMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var translationCulture = new CultureInfo(cultureName);

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<IFoodGroup, object>(translatableMock, translationCulture);

            translatableMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationCulture)));
        }

        /// <summary>
        /// Tests that Map calls Translate on each translatable domain object in source if source is a collection of translatable domain objects and the translation culture is null.
        /// </summary>
        [Test]
        public void TestThatMapCallsTranslateOnEachTranslatableInSourceIfSourceIsCollectionOfTranslatablesAndTranslationCultureIsNull()
        {
            var translatableMockCollection = DomainObjectMockBuilder.BuildFoodGroupMockCollection().ToList();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<List<IFoodGroup>, IEnumerable<object>>(translatableMockCollection);

            translatableMockCollection.ForEach(m => m.AssertWasCalled(n => n.Translate(Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture))));
        }

        /// <summary>
        /// Tests that Map calls Translate on each translatable domain object in source if source is a collection of translatable domain objects and the translation culture is not null.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        public void TestThatMapCallsTranslateOnEachTranslatableInSourceIfSourceIsCollectionOfTranslatablesAndTranslationCultureIsNotNull(string cultureName)
        {
            var translatableMockCollection = DomainObjectMockBuilder.BuildFoodGroupMockCollection().ToList();
            var translationCulture = new CultureInfo(cultureName);

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<List<IFoodGroup>, IEnumerable<object>>(translatableMockCollection, translationCulture);

            translatableMockCollection.ForEach(m => m.AssertWasCalled(n => n.Translate(Arg<CultureInfo>.Is.Equal(translationCulture))));
        }

        /// <summary>
        /// Tests that Map maps TranslationInfo to TranslationInfoSystemView.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        public void TestThatMapMapsTranslationInfoToTranslationInfoSystemView(string cultureName)
        {
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock(cultureName);

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var translationInfoSystemView = foodWasteObjectMapper.Map<ITranslationInfo, TranslationInfoSystemView>(translationInfoMock);
            Assert.That(translationInfoSystemView.TranslationInfoIdentifier, Is.Not.Null);
            Assert.That(translationInfoSystemView.TranslationInfoIdentifier, Is.EqualTo(translationInfoMock.Identifier.HasValue ? translationInfoMock.Identifier.Value : Guid.Empty));
            Assert.That(translationInfoSystemView.CultureName, Is.Not.Null);
            Assert.That(translationInfoSystemView.CultureName, Is.Not.Empty);
            Assert.That(translationInfoSystemView.CultureName, Is.EqualTo(cultureName));
        }
    }
}
