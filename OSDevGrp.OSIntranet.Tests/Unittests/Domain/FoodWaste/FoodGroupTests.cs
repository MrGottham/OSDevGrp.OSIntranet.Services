using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the food group.
    /// </summary>
    [TestFixture]
    public class FoodGroupTests
    {
        /// <summary>
        /// Private class for testing the food group domain object.
        /// </summary>
        private class MyFoodGroup : FoodGroup
        {
            #region Properties

            /// <summary>
            /// Foods groups which has this food group as a parent. 
            /// </summary>
            public new IEnumerable<IFoodGroup> Children
            {
                get { return base.Children; }
                set { base.Children = value; }
            }

            #endregion
        }
        /// <summary>
        /// Tests that the constructor initialize a food group.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeForeignKey()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Identifier, Is.Null);
            Assert.That(foodGroup.Identifier.HasValue, Is.False);
            Assert.That(foodGroup.Parent, Is.Null);
            Assert.That(foodGroup.IsActive, Is.False);
            Assert.That(foodGroup.Children, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Empty);
            Assert.That(foodGroup.Translation, Is.Null);
            Assert.That(foodGroup.Translations, Is.Not.Null);
            Assert.That(foodGroup.Translations, Is.Empty);
            Assert.That(foodGroup.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroup.ForeignKeys, Is.Empty);
        }

        /// <summary>
        /// Tests that the setter to Parent sets new value which are not null.
        /// </summary>
        [Test]
        public void TestThatParentSetterSetsValueWhichAreNotNull()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Null);

            var parentFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();

            foodGroup.Parent = parentFoodGroupMock;
            Assert.That(foodGroup.Parent, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.EqualTo(parentFoodGroupMock));
        }

        /// <summary>
        /// Tests that the setter to Parent sets new value which are null.
        /// </summary>
        [Test]
        public void TestThatParentSetterSetsValueWhichAreNull()
        {
            var foodGroup = new FoodGroup
            {
                Parent = DomainObjectMockBuilder.BuildFoodGroupMock()
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Not.Null);

            foodGroup.Parent = null;
            Assert.That(foodGroup.Parent, Is.Null);
        }

        /// <summary>
        /// Tests that the setter to Parent throws an ArgumentException when the new value has no identifier.
        /// </summary>
        [Test]
        public void TestThatParentSetterThrowsArgumentExceptionWhenValueHasNoIdentifier()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Null);

            var exception = Assert.Throws<ArgumentException>(() => foodGroup.Parent = new FoodGroup {Identifier = null});
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            // ReSharper disable NotResolvedInText
            Assert.That(exception.Message, Is.EqualTo((new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Identifier"), "value")).Message));
            // ReSharper restore NotResolvedInText
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter to Parent throws an ArgumentException when the new value equels the food group which updates Parent.
        /// </summary>
        [Test]
        public void TestThatParentSetterThrowsArgumentExceptionWhenValueEqualsFoodGroupWhichUpdatesParent()
        {
            var foodGroup = new FoodGroup
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Identifier, Is.Not.Null);
            Assert.That(foodGroup.Identifier.HasValue, Is.True);
            Assert.That(foodGroup.Parent, Is.Null);

            var exception = Assert.Throws<ArgumentException>(() => foodGroup.Parent = foodGroup);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            // ReSharper disable NotResolvedInText
            Assert.That(exception.Message, Is.EqualTo((new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroup, "value"), "value")).Message));
            // ReSharper restore NotResolvedInText
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter to Parent throws an ArgumentException when the new value makes a circular reference.
        /// </summary>
        [Test]
        public void TestThatParentSetterThrowsArgumentExceptionWhenValueMakesCurcularReference()
        {
            var foodGroupLevel1 = new FoodGroup
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupLevel1, Is.Not.Null);
            Assert.That(foodGroupLevel1.Identifier, Is.Not.Null);
            Assert.That(foodGroupLevel1.Identifier.HasValue, Is.True);
            Assert.That(foodGroupLevel1.Parent, Is.Null);

            var foodGroupLevel2 = new FoodGroup
            {
                Identifier = Guid.NewGuid(),
                Parent = foodGroupLevel1
            };
            Assert.That(foodGroupLevel2, Is.Not.Null);
            Assert.That(foodGroupLevel2.Identifier, Is.Not.Null);
            Assert.That(foodGroupLevel2.Identifier.HasValue, Is.True);
            Assert.That(foodGroupLevel2.Parent, Is.Not.Null);
            Assert.That(foodGroupLevel2.Parent, Is.EqualTo(foodGroupLevel1));

            var foodGroupLevel3 = new FoodGroup
            {
                Identifier = Guid.NewGuid(),
                Parent = foodGroupLevel2
            };
            Assert.That(foodGroupLevel3, Is.Not.Null);
            Assert.That(foodGroupLevel3.Identifier, Is.Not.Null);
            Assert.That(foodGroupLevel3.Identifier.HasValue, Is.True);
            Assert.That(foodGroupLevel3.Parent, Is.Not.Null);
            Assert.That(foodGroupLevel3.Parent, Is.EqualTo(foodGroupLevel2));

            var exception = Assert.Throws<ArgumentException>(() => foodGroupLevel1.Parent = foodGroupLevel3);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            // ReSharper disable NotResolvedInText
            Assert.That(exception.Message, Is.EqualTo((new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroupLevel3, "value"), "value").Message)));
            // ReSharper restore NotResolvedInText
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter to IsActive sets new value to true.
        /// </summary>
        [Test]
        public void TestThatIsActiveSetterSetsValueToTrue()
        {
            var foodGroup = new FoodGroup
            {
                IsActive = false
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.IsActive, Is.False);

            foodGroup.IsActive = true;
            Assert.That(foodGroup.IsActive, Is.True);
        }

        /// <summary>
        /// Tests that the setter to IsActive sets new value to false.
        /// </summary>
        [Test]
        public void TestThatIsActiveSetterSetsValueToFalse()
        {
            var foodGroup = new FoodGroup
            {
                IsActive = true
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.IsActive, Is.True);

            foodGroup.IsActive = false;
            Assert.That(foodGroup.IsActive, Is.False);
        }

        /// <summary>
        /// Tests that the setter to Children throws an ArgumentNullException when the new value is null.
        /// </summary>
        [Test]
        public void TestThatChildrenSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            var foodGroup = new MyFoodGroup();
            Assert.That(foodGroup, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroup.Children = null);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationAdd adds a translation.
        /// </summary>
        [Test]
        public void TestThatTranslationAddAddsTranslation()
        {
            var foodGroup = new FoodGroup
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Translations, Is.Not.Null);
            Assert.That(foodGroup.Translations, Is.Empty);
            
            // ReSharper disable PossibleInvalidOperationException
            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(foodGroup.Identifier.Value);
            // ReSharper restore PossibleInvalidOperationException

            foodGroup.TranslationAdd(translationMock);
            Assert.That(foodGroup.Translations, Is.Not.Null);
            Assert.That(foodGroup.Translations, Is.Not.Empty);
            Assert.That(foodGroup.Translations.Count(), Is.EqualTo(1));
            Assert.That(foodGroup.Translations.Contains(translationMock), Is.True);
        }

        /// <summary>
        /// Tests that TranslationAdd throws an ArgumentNullException when the translation if null.
        /// </summary>
        [Test]
        public void TestThatTranslationAddThrowsArgumentNullExceptionWhenTranslationIsNull()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroup.TranslationAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyAdd adds a foreign key.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddAddsForeignKey()
        {
            var foodGroup = new FoodGroup
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroup.ForeignKeys, Is.Empty);

            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroup.Identifier.Value, foodGroup.GetType());
            // ReSharper restore PossibleInvalidOperationException

            foodGroup.ForeignKeyAdd(foreignKeyMock);
            Assert.That(foodGroup.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroup.ForeignKeys, Is.Not.Empty);
            Assert.That(foodGroup.ForeignKeys.Count(), Is.EqualTo(1));
            Assert.That(foodGroup.ForeignKeys.Contains(foreignKeyMock), Is.True);
        }

        /// <summary>
        /// Tests that ForeignKeyAdd throws an ArgumentNullException when the foreign key if null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddThrowsArgumentNullExceptionWhenForeignKeyIsNull()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroup.ForeignKeyAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKey"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RemoveInactiveChildren removes inactive children.
        /// </summary>
        [Test]
        public void TestThatRemoveInactiveChildrenRemovesInactiveChildren()
        {
            var activeFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup1.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup2.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup3.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var inactiveFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup1.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup2.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup3.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();

            var foodGroup = new MyFoodGroup
            {
                Children = new List<IFoodGroup>
                {
                    activeFoodGroup1,
                    inactiveFoodGroup1,
                    activeFoodGroup2,
                    inactiveFoodGroup2,
                    activeFoodGroup3,
                    inactiveFoodGroup3
                }
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Empty);
            Assert.That(foodGroup.Children.Count(), Is.EqualTo(6));

            foodGroup.RemoveInactiveChildren();
            Assert.That(foodGroup.Children, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Empty);
            Assert.That(foodGroup.Children.Count(), Is.EqualTo(3));
            Assert.That(foodGroup.Children.Contains(activeFoodGroup1), Is.True);
            Assert.That(foodGroup.Children.Contains(activeFoodGroup2), Is.True);
            Assert.That(foodGroup.Children.Contains(activeFoodGroup2), Is.True);
        }

        /// <summary>
        /// Tests that RemoveInactiveChildren calls RemoveInactiveChildren on active children.
        /// </summary>
        [Test]
        public void TestThatRemoveInactiveChildrenCallsRemoveInactiveChildrenOnActiveChildrens()
        {
            var activeFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup1.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup2.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup3.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var inactiveFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup1.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup2.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup3.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();

            var foodGroup = new MyFoodGroup
            {
                Children = new List<IFoodGroup>
                {
                    activeFoodGroup1,
                    inactiveFoodGroup1,
                    activeFoodGroup2,
                    inactiveFoodGroup2,
                    activeFoodGroup3,
                    inactiveFoodGroup3
                }
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Empty);

            foodGroup.RemoveInactiveChildren();

            activeFoodGroup1.AssertWasCalled(m => m.RemoveInactiveChildren());
            activeFoodGroup2.AssertWasCalled(m => m.RemoveInactiveChildren());
            activeFoodGroup3.AssertWasCalled(m => m.RemoveInactiveChildren());
        }

        /// <summary>
        /// Tests that RemoveInactiveChildren does not call RemoveInactiveChildren on inactive children.
        /// </summary>
        [Test]
        public void TestThatRemoveInactiveChildrenDoesNotCallRemoveInactiveChildrenOnInactiveChildrens()
        {
            var activeFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup1.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup2.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup3.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var inactiveFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup1.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup2.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup3.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();

            var foodGroup = new MyFoodGroup
            {
                Children = new List<IFoodGroup>
                {
                    activeFoodGroup1,
                    inactiveFoodGroup1,
                    activeFoodGroup2,
                    inactiveFoodGroup2,
                    activeFoodGroup3,
                    inactiveFoodGroup3
                }
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Empty);

            foodGroup.RemoveInactiveChildren();

            inactiveFoodGroup1.AssertWasNotCalled(m => m.RemoveInactiveChildren());
            inactiveFoodGroup2.AssertWasNotCalled(m => m.RemoveInactiveChildren());
            inactiveFoodGroup3.AssertWasNotCalled(m => m.RemoveInactiveChildren());
        }

        /// <summary>
        /// Test that Translate calls Translate on the parent food group when the parent food group is not null and has not been translated.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnParentWhenParentIsNotNullAndHasNotBeenTranslated()
        {
            var cultureInfo = CultureInfo.CurrentUICulture;

            var parentFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            parentFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            parentFoodGroupMock.Stub(m => m.Translation)
                .Return(null)
                .Repeat.Any();
            
            var foodGroup = new FoodGroup
            {
                Parent = parentFoodGroupMock
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.EqualTo(parentFoodGroupMock));
            Assert.That(foodGroup.Parent.Translation, Is.Null);

            foodGroup.Translate(cultureInfo);

            parentFoodGroupMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(cultureInfo)));
        }

        /// <summary>
        /// Test that Translate does not call Translate on the parent food group when the parent food group is not null and has been translated.
        /// </summary>
        [Test]
        public void TestThatTranslateDoesNotCallTranslateOnParentWhenParentIsNotNullAndHasBeenTranslated()
        {
            var cultureInfo = CultureInfo.CurrentUICulture;

            var parentFoodGroupIdentifier = Guid.NewGuid();
            var parentFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            parentFoodGroupMock.Stub(m => m.Identifier)
                .Return(parentFoodGroupIdentifier)
                .Repeat.Any();
            parentFoodGroupMock.Stub(m => m.Translation)
                .Return(DomainObjectMockBuilder.BuildTranslationMock(parentFoodGroupIdentifier))
                .Repeat.Any();

            var foodGroup = new FoodGroup
            {
                Parent = parentFoodGroupMock
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.EqualTo(parentFoodGroupMock));
            Assert.That(foodGroup.Parent.Translation, Is.Not.Null);

            foodGroup.Translate(cultureInfo);

            parentFoodGroupMock.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Test that Translate calls Translate on each children which has not been translated.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnEndChildrenWhichHasNotBeenTranslated()
        {
            var cultureInfo = CultureInfo.CurrentUICulture;

            var untranslatedFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            untranslatedFoodGroup1.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            untranslatedFoodGroup1.Stub(m => m.Translation)
                .Return(null)
                .Repeat.Any();
            var untranslatedFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            untranslatedFoodGroup2.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            untranslatedFoodGroup2.Stub(m => m.Translation)
                .Return(null)
                .Repeat.Any();
            var untranslatedFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            untranslatedFoodGroup3.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            untranslatedFoodGroup3.Stub(m => m.Translation)
                .Return(null)
                .Repeat.Any();
            var translatedFoodGroup1Identifier = Guid.NewGuid();
            var translatedFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            translatedFoodGroup1.Stub(m => m.Identifier)
                .Return(translatedFoodGroup1Identifier)
                .Repeat.Any();
            translatedFoodGroup1.Stub(m => m.Translation)
                .Return(DomainObjectMockBuilder.BuildTranslationMock(translatedFoodGroup1Identifier))
                .Repeat.Any();
            var translatedFoodGroup2Identifier = Guid.NewGuid();
            var translatedFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            translatedFoodGroup2.Stub(m => m.Identifier)
                .Return(translatedFoodGroup2Identifier)
                .Repeat.Any();
            translatedFoodGroup2.Stub(m => m.Translation)
                .Return(DomainObjectMockBuilder.BuildTranslationMock(translatedFoodGroup2Identifier))
                .Repeat.Any();
            var translatedFoodGroup3Identifier = Guid.NewGuid();
            var translatedFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            translatedFoodGroup3.Stub(m => m.Identifier)
                .Return(translatedFoodGroup3Identifier)
                .Repeat.Any();
            translatedFoodGroup3.Stub(m => m.Translation)
                .Return(DomainObjectMockBuilder.BuildTranslationMock(translatedFoodGroup3Identifier))
                .Repeat.Any();

            var foodGroup = new MyFoodGroup
            {
                Children = new List<IFoodGroup>
                {
                    untranslatedFoodGroup1,
                    translatedFoodGroup1,
                    untranslatedFoodGroup2,
                    translatedFoodGroup2,
                    untranslatedFoodGroup3,
                    translatedFoodGroup3
                }
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Empty);
            
            foodGroup.Translate(cultureInfo);

            untranslatedFoodGroup1.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(cultureInfo)));
            untranslatedFoodGroup2.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(cultureInfo)));
            untranslatedFoodGroup3.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(cultureInfo)));
        }

        /// <summary>
        /// Test that Translate does not call Translate on each children which has been translated.
        /// </summary>
        [Test]
        public void TestThatTranslateDoesNotCallTranslateOnEndChildrenWhichHasBeenTranslated()
        {
            var cultureInfo = CultureInfo.CurrentUICulture;

            var untranslatedFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            untranslatedFoodGroup1.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            untranslatedFoodGroup1.Stub(m => m.Translation)
                .Return(null)
                .Repeat.Any();
            var untranslatedFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            untranslatedFoodGroup2.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            untranslatedFoodGroup2.Stub(m => m.Translation)
                .Return(null)
                .Repeat.Any();
            var untranslatedFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            untranslatedFoodGroup3.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            untranslatedFoodGroup3.Stub(m => m.Translation)
                .Return(null)
                .Repeat.Any();
            var translatedFoodGroup1Identifier = Guid.NewGuid();
            var translatedFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            translatedFoodGroup1.Stub(m => m.Identifier)
                .Return(translatedFoodGroup1Identifier)
                .Repeat.Any();
            translatedFoodGroup1.Stub(m => m.Translation)
                .Return(DomainObjectMockBuilder.BuildTranslationMock(translatedFoodGroup1Identifier))
                .Repeat.Any();
            var translatedFoodGroup2Identifier = Guid.NewGuid();
            var translatedFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            translatedFoodGroup2.Stub(m => m.Identifier)
                .Return(translatedFoodGroup2Identifier)
                .Repeat.Any();
            translatedFoodGroup2.Stub(m => m.Translation)
                .Return(DomainObjectMockBuilder.BuildTranslationMock(translatedFoodGroup2Identifier))
                .Repeat.Any();
            var translatedFoodGroup3Identifier = Guid.NewGuid();
            var translatedFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            translatedFoodGroup3.Stub(m => m.Identifier)
                .Return(translatedFoodGroup3Identifier)
                .Repeat.Any();
            translatedFoodGroup3.Stub(m => m.Translation)
                .Return(DomainObjectMockBuilder.BuildTranslationMock(translatedFoodGroup3Identifier))
                .Repeat.Any();

            var foodGroup = new MyFoodGroup
            {
                Children = new List<IFoodGroup>
                {
                    untranslatedFoodGroup1,
                    translatedFoodGroup1,
                    untranslatedFoodGroup2,
                    translatedFoodGroup2,
                    untranslatedFoodGroup3,
                    translatedFoodGroup3
                }
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Not.Empty);

            foodGroup.Translate(cultureInfo);

            translatedFoodGroup1.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything));
            translatedFoodGroup2.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything));
            translatedFoodGroup3.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything));
        }
    }
}
