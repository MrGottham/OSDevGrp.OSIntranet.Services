using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Dispatchers;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Dispatchers
{
    /// <summary>
    /// Tests the functionality which can merge fields in a static text.
    /// </summary>
    public class StaticTextFieldMergeTests
    {
        /// <summary>
        /// Tests that the constructor initialize the functionality which can merge fields in a static text.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStaticTextFieldMerge()
        {
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);
            Assert.That(staticTextFieldMerge.MergeFields, Is.Not.Null);
            Assert.That(staticTextFieldMerge.MergeFields, Is.Not.Empty);
            Assert.That(staticTextFieldMerge.MergeFields.Count(), Is.EqualTo(2));
            Assert.That(staticTextFieldMerge.MergeFields.Any(keyValuePair => String.Compare(keyValuePair.Key, "[PrivacyPoliciesSubject]", StringComparison.Ordinal) == 0), Is.True);
            Assert.That(staticTextFieldMerge.MergeFields.Any(keyValuePair => String.Compare(keyValuePair.Key, "[PrivacyPoliciesBody]", StringComparison.Ordinal) == 0), Is.True);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new StaticTextFieldMerge(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("systemDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Merge throws an ArgumentNullException when the static text is null.
        /// </summary>
        [Test]
        public void TestThatMergeThrowsArgumentNullExceptionWhenStaticTextIsNull()
        {
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => staticTextFieldMerge.Merge(null, DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("staticText"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Merge throws an ArgumentNullException when the translation informations used to translate the static text is null.
        /// </summary>
        [Test]
        public void TestThatMergeThrowsArgumentNullExceptionWhenTranslationInfoIsNull()
        {
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => staticTextFieldMerge.Merge(DomainObjectMockBuilder.BuildStaticTextMock(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Merge calls Translate on the static text so it will be translated.
        /// </summary>
        [Test]
        public void TestThatMergeCallsTranslateOnStaticText()
        {
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();

            var staticTextIdentifier = Guid.NewGuid();
            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Stub(m => m.SubjectTranslation)
                .Return(DomainObjectMockBuilder.BuildTranslationMock(staticTextIdentifier))
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslation)
                .Return(DomainObjectMockBuilder.BuildTranslationMock(staticTextIdentifier))
                .Repeat.Any();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);

            staticTextFieldMerge.Merge(staticTextMock, translationInfoMock);

            staticTextMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Merge returns without error when both SubjectTranslation and BodyTranslation on the static text is null.
        /// </summary>
        [Test]
        public void TestThatMergeReturnsWithoutErrorWhenSubjectTranslationIsNullAndBodyTranslationIsNullOnStaticText()
        {
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();

            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Stub(m => m.SubjectTranslation)
                .Return(null)
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslation)
                .Return(null)
                .Repeat.Any();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);

            staticTextFieldMerge.Merge(staticTextMock, DomainObjectMockBuilder.BuildTranslationInfoMock());
        }

        /// <summary>
        /// Tests that Merge returns without error when SubjectTranslation has no value and BodyTranslation is null on the static text.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatMergeReturnsWithoutErrorWhenSubjectTranslationHasTranslationWithoutValueAndBodyTranslationIsNullOnStaticText(string testValue)
        {
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();

            var subjectTranslationMock = MockRepository.GenerateMock<ITranslation>();
            subjectTranslationMock.Stub(m => m.Value)
                .Return(testValue)
                .Repeat.Any();

            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Stub(m => m.SubjectTranslation)
                .Return(subjectTranslationMock)
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslation)
                .Return(null)
                .Repeat.Any();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);

            staticTextFieldMerge.Merge(staticTextMock, DomainObjectMockBuilder.BuildTranslationInfoMock());

            subjectTranslationMock.AssertWasNotCalled(m => m.Value = Arg<string>.Is.Anything);
        }

        /// <summary>
        /// Tests that Merge returns without error when SubjectTranslation is null and BodyTranslation has no value on the static text.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatMergeReturnsWithoutErrorWhenSubjectTranslationIsNullAndBodyTranslationHasTranslationWithoutValue(string testValue)
        {
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();

            var bodyTranslationMock = MockRepository.GenerateMock<ITranslation>();
            bodyTranslationMock.Stub(m => m.Value)
                .Return(testValue)
                .Repeat.Any();

            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Stub(m => m.SubjectTranslation)
                .Return(null)
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslation)
                .Return(bodyTranslationMock)
                .Repeat.Any();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);

            staticTextFieldMerge.Merge(staticTextMock, DomainObjectMockBuilder.BuildTranslationInfoMock());

            bodyTranslationMock.AssertWasNotCalled(m => m.Value = Arg<string>.Is.Anything);
        }

        /// <summary>
        /// Tests that Merge calls StaticTextGetByStaticTextType on the repository which can access system data for the food waste domain when a static text need to be resolved.
        /// </summary>
        [Test]
        [TestCase("[PrivacyPoliciesSubject]", StaticTextType.PrivacyPolicy)]
        [TestCase("[PrivacyPoliciesBody]", StaticTextType.PrivacyPolicy)]
        public void TestThatMergeCallsStaticTextGetByStaticTextTypeWhenStaticTextNeedToBeResolved(string mergeField, StaticTextType staticTextType)
        {
            var fixture = new Fixture();
            
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepository.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildStaticTextMock())
                .Repeat.Any();

            var subjectTranslationMock = MockRepository.GenerateMock<ITranslation>();
            subjectTranslationMock.Stub(m => m.Value)
                .Return(string.Format("{0}{1}{2}", fixture.Create<string>(), mergeField, fixture.Create<string>()))
                .Repeat.Any();

            var bodyTranslationMock = MockRepository.GenerateMock<ITranslation>();
            bodyTranslationMock.Stub(m => m.Value)
                .Return(string.Format("{0}{1}{2}", fixture.Create<string>(), mergeField, fixture.Create<string>()))
                .Repeat.Any();

            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Stub(m => m.SubjectTranslation)
                .Return(subjectTranslationMock)
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslation)
                .Return(bodyTranslationMock)
                .Repeat.Any();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);

            staticTextFieldMerge.Merge(staticTextMock, DomainObjectMockBuilder.BuildTranslationInfoMock());

            systemDataRepository.AssertWasCalled(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Equal(staticTextType)), opt => opt.Repeat.Times(1));
        }

        /// <summary>
        /// Tests that Merge calls Translation on the static text which need to be resolved.
        /// </summary>
        [Test]
        [TestCase("[PrivacyPoliciesSubject]", StaticTextType.PrivacyPolicy)]
        [TestCase("[PrivacyPoliciesBody]", StaticTextType.PrivacyPolicy)]
        public void TestThatMergeCallsTranslationOnStaticTextWhichNeedToBeResolved(string mergeField, StaticTextType staticTextType)
        {
            var fixture = new Fixture();

            var resolveStaticTextMock = DomainObjectMockBuilder.BuildStaticTextMock(staticTextType);
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepository.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(resolveStaticTextMock)
                .Repeat.Any();

            var subjectTranslationMock = MockRepository.GenerateMock<ITranslation>();
            subjectTranslationMock.Stub(m => m.Value)
                .Return(string.Format("{0}{1}{2}", fixture.Create<string>(), mergeField, fixture.Create<string>()))
                .Repeat.Any();

            var bodyTranslationMock = MockRepository.GenerateMock<ITranslation>();
            bodyTranslationMock.Stub(m => m.Value)
                .Return(string.Format("{0}{1}{2}", fixture.Create<string>(), mergeField, fixture.Create<string>()))
                .Repeat.Any();

            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Stub(m => m.SubjectTranslation)
                .Return(subjectTranslationMock)
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslation)
                .Return(bodyTranslationMock)
                .Repeat.Any();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);

            staticTextFieldMerge.Merge(staticTextMock, translationInfoMock);

            resolveStaticTextMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Merge merges the value from the static text which need to be resolved into the static text.
        /// </summary>
        [Test]
        [TestCase("[PrivacyPoliciesSubject]")]
        [TestCase("[PrivacyPoliciesBody]")]
        public void TestThatMergeMergesValueFromStaticTextWhichNeedToBeResolved(string mergeField)
        {
            var fixture = new Fixture();

            var replaceWithValue = fixture.Create<string>();
            var replaceWithTranslation = MockRepository.GenerateMock<ITranslation>();
            replaceWithTranslation.Stub(m => m.Value)
                .Return(replaceWithValue)
                .Repeat.Any();

            var resolveStaticTextMock = MockRepository.GenerateMock<IStaticText>();
            resolveStaticTextMock.Stub(m => m.SubjectTranslation)
                .Return(replaceWithTranslation)
                .Repeat.Any();
            resolveStaticTextMock.Stub(m => m.BodyTranslation)
                .Return(replaceWithTranslation)
                .Repeat.Any();
            resolveStaticTextMock.Stub(m => m.Translations)
                .Return(new List<ITranslation> {replaceWithTranslation})
                .Repeat.Any();

            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepository.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(resolveStaticTextMock)
                .Repeat.Any();

            var subjectTranslation = string.Format("{0}{1}{2}", fixture.Create<string>(), mergeField, fixture.Create<string>());
            var subjectTranslationMock = MockRepository.GenerateMock<ITranslation>();
            subjectTranslationMock.Stub(m => m.Value)
                .Return(subjectTranslation)
                .Repeat.Any();

            var bodyTranslation = string.Format("{0}{1}{2}", fixture.Create<string>(), mergeField, fixture.Create<string>());
            var bodyTranslationMock = MockRepository.GenerateMock<ITranslation>();
            bodyTranslationMock.Stub(m => m.Value)
                .Return(bodyTranslation)
                .Repeat.Any();

            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Stub(m => m.SubjectTranslation)
                .Return(subjectTranslationMock)
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslation)
                .Return(bodyTranslationMock)
                .Repeat.Any();

            var staticTextFieldMerge = new StaticTextFieldMerge(systemDataRepository);
            Assert.That(staticTextFieldMerge, Is.Not.Null);

            staticTextFieldMerge.Merge(staticTextMock, DomainObjectMockBuilder.BuildTranslationInfoMock());

            subjectTranslationMock.AssertWasCalled(m => m.Value = Arg<string>.Is.Equal(subjectTranslation.Replace(mergeField, replaceWithValue)));
            bodyTranslationMock.AssertWasCalled(m => m.Value = Arg<string>.Is.Equal(bodyTranslation.Replace(mergeField, replaceWithValue)));
        }
    }
}
