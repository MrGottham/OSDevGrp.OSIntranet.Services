﻿using System;
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
        /// Tests that FoodGroupGetAll gets all the food groups.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllGetsFoodGroups()
        {
            var foodGroupAtRoot = _systemDataRepository.Insert(new FoodGroup { Parent = null, IsActive = true });
            try
            {
                var foodGroupWithParent = _systemDataRepository.Insert(new FoodGroup { Parent = foodGroupAtRoot, IsActive = true });
                try
                {
                    var result = _systemDataRepository.FoodGroupGetAll().ToList();
                    Assert.That(result, Is.Not.Null);
                    Assert.That(result, Is.Not.Empty);
                    Assert.That(result.Any(m => m.Identifier.HasValue && m.Identifier.Equals(foodGroupAtRoot.Identifier)), Is.True);
                    Assert.That(result.Any(m => m.Identifier.HasValue && m.Identifier.Equals(foodGroupWithParent.Identifier)), Is.True);
                }
                finally
                {
                    _systemDataRepository.Delete(foodGroupWithParent);
                }
            }
            finally
            {
                _systemDataRepository.Delete(foodGroupAtRoot);
            }
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot gets all the food groups at the root level.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootGetsFoodGroupsAtRootLevel()
        {
            var foodGroupAtRoot = _systemDataRepository.Insert(new FoodGroup {Parent = null, IsActive = true});
            try
            {
                var foodGroupWithParent = _systemDataRepository.Insert(new FoodGroup {Parent = foodGroupAtRoot, IsActive = true});
                try
                {
                    var result = _systemDataRepository.FoodGroupGetAllOnRoot().ToList();
                    Assert.That(result, Is.Not.Null);
                    Assert.That(result, Is.Not.Empty);
                    Assert.That(result.Any(m => m.Identifier.HasValue && m.Identifier.Equals(foodGroupAtRoot.Identifier)), Is.True);
                    Assert.That(result.Any(m => m.Identifier.HasValue && m.Identifier.Equals(foodGroupWithParent.Identifier)), Is.False);
                }
                finally
                {
                    _systemDataRepository.Delete(foodGroupWithParent);
                }
            }
            finally
            {
                _systemDataRepository.Delete(foodGroupAtRoot);
            }
        }

        /// <summary>
        /// Tests that Get gets a given food group.
        /// </summary>
        [Test]
        public void TestThatGetGetsFoodGroup()
        {
            var foodGroupAtRoot = _systemDataRepository.Insert(new FoodGroup {Parent = null, IsActive = true});
            try
            {
                var foodGroupWithParent = _systemDataRepository.Insert(new FoodGroup {Parent = foodGroupAtRoot, IsActive = true});
                try
                {
                    // ReSharper disable PossibleInvalidOperationException
                    var result = _systemDataRepository.Get<IFoodGroup>(foodGroupWithParent.Identifier.Value);
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(result.Parent, Is.Not.Null);
                    Assert.That(result.IsActive, Is.True);
                }
                finally
                {
                    _systemDataRepository.Delete(foodGroupWithParent);
                }
            }
            finally
            {
                _systemDataRepository.Delete(foodGroupAtRoot);
            }
        }

        /// <summary>
        /// Tests that Update updates a given food group.
        /// </summary>
        [Test]
        public void TestThatUpdateUpdatesFoodGroup()
        {
            var foodGroupAtRoot = _systemDataRepository.Insert(new FoodGroup {Parent = null, IsActive = true});
            try
            {
                var foodGroupWithParent = _systemDataRepository.Insert(new FoodGroup {Parent = foodGroupAtRoot, IsActive = true});
                try
                {
                    foodGroupWithParent.Parent = null;
                    foodGroupWithParent.IsActive = false;
                    _systemDataRepository.Update(foodGroupWithParent);
                    
                    // ReSharper disable PossibleInvalidOperationException
                    var result = _systemDataRepository.Get<IFoodGroup>(foodGroupWithParent.Identifier.Value);
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(result.Parent, Is.Null);
                    Assert.That(result.IsActive, Is.False);
                }
                finally
                {
                    _systemDataRepository.Delete(foodGroupWithParent);
                }
            }
            finally
            {
                _systemDataRepository.Delete(foodGroupAtRoot);
            }
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet gets foreign keys for a given identifiable domain object.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetGetsForeignKeysForIdentifiableDomainObject()
        {
            var dataProvider = _systemDataRepository.DataProviderForFoodsGet();
            var foreignKeyFor = new ForeignKey(dataProvider, Guid.NewGuid(), typeof (ForeignKey), "Test")
            {
                Identifier = Guid.NewGuid()
            };
            // ReSharper disable PossibleInvalidOperationException
            var foreignKey = _systemDataRepository.Insert(new ForeignKey(dataProvider, foreignKeyFor.Identifier.Value, foreignKeyFor.GetType(), "Test"));
            // ReSharper restore PossibleInvalidOperationException
            try
            {
                var result = _systemDataRepository.ForeignKeysForDomainObjectGet(foreignKeyFor);
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.Not.Empty);
            }
            finally
            {
                _systemDataRepository.Delete(foreignKey);
            }
        }

        /// <summary>
        /// Tests that Get gets a given foreign key.
        /// </summary>
        [Test]
        public void TestThatGetGetsForeignKey()
        {
            var dataProvider = _systemDataRepository.DataProviderForFoodsGet();
            var foreignKey = _systemDataRepository.Insert(new ForeignKey(dataProvider, Guid.NewGuid(), typeof (ForeignKey), "Test"));
            try
            {
                // ReSharper disable PossibleInvalidOperationException
                var result = _systemDataRepository.Get<IForeignKey>(foreignKey.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Identifier, Is.EqualTo(foreignKey.Identifier));
            }
            finally
            {
                _systemDataRepository.Delete(foreignKey);
            }
        }

        /// <summary>
        /// Tests that Update updates a given foreign key.
        /// </summary>
        [Test]
        public void TestThatUpdateUpdatesForeignKey()
        {
            var dataProvider = _systemDataRepository.DataProviderForFoodsGet();
            var foreignKey = _systemDataRepository.Insert(new ForeignKey(dataProvider, Guid.NewGuid(), typeof(ForeignKey), "Test"));
            try
            {
                foreignKey.ForeignKeyValue = "Testing";
                _systemDataRepository.Update(foreignKey);

                // ReSharper disable PossibleInvalidOperationException
                var result = _systemDataRepository.Get<IForeignKey>(foreignKey.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.ForeignKeyValue, Is.Not.Null);
                Assert.That(result.ForeignKeyValue, Is.Not.Empty);
                Assert.That(result.ForeignKeyValue, Is.EqualTo(foreignKey.ForeignKeyValue));
            }
            finally
            {
                _systemDataRepository.Delete(foreignKey);
            }
        }

        /// <summary>
        /// Tests that DataProviderForFoodsGet returns the default data provider for foods.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodsGetReturnsDataProvider()
        {
            var dataProvider = _systemDataRepository.DataProviderForFoodsGet();
            Assert.That(dataProvider, Is.Not.Null);
        }

        /// <summary>
        /// Tests that DataProviderForFoodGroupsGet returns the default data provider for food groups.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodGroupsGetReturnsDataProvider()
        {
            var dataProvider = _systemDataRepository.DataProviderForFoodGroupsGet();
            Assert.That(dataProvider, Is.Not.Null);
        }

        /// <summary>
        /// Tests that DataProviderGetAll returns all the data providers.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllReturnsDataProviders()
        {
            var dataProviders = _systemDataRepository.DataProviderGetAll();
            Assert.That(dataProviders, Is.Not.Null);
            Assert.That(dataProviders, Is.Not.Empty);
            Assert.That(dataProviders.Count(), Is.EqualTo(1));
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
        /// Tests that Get gets a given translation.
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
        /// Tests that Update updates a given translation.
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
