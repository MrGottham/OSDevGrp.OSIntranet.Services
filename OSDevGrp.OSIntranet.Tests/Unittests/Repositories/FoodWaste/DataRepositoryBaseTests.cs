using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.FoodWaste
{
    /// <summary>
    /// Tests the basic functionality to repositories in the food waste domain.
    /// </summary>
    [TestFixture]
    public class DataRepositoryBaseTests
    {
        private static readonly IList<Tuple<Type, Type, Type>> TypesToTest = new List<Tuple<Type, Type, Type>>
        {
            new Tuple<Type, Type, Type>(typeof(IHousehold), typeof(IHouseholdProxy), typeof(HouseholdProxy)),
            new Tuple<Type, Type, Type>(typeof(IStorage), typeof(IStorageProxy), typeof(StorageProxy)),
            new Tuple<Type, Type, Type>(typeof(IStorageType), typeof(IStorageTypeProxy), typeof(StorageTypeProxy)),
            new Tuple<Type, Type, Type>(typeof(IHouseholdMember), typeof(IHouseholdMemberProxy), typeof(HouseholdMemberProxy)),
            new Tuple<Type, Type, Type>(typeof(IPayment), typeof(IPaymentProxy), typeof(PaymentProxy)),
            new Tuple<Type, Type, Type>(typeof(IFoodItem), typeof(IFoodItemProxy), typeof(FoodItemProxy)),
            new Tuple<Type, Type, Type>(typeof(IFoodGroup), typeof(IFoodGroupProxy), typeof(FoodGroupProxy)),
            new Tuple<Type, Type, Type>(typeof(IForeignKey), typeof(IForeignKeyProxy), typeof(ForeignKeyProxy)),
            new Tuple<Type, Type, Type>(typeof(IStaticText), typeof(IStaticTextProxy), typeof(StaticTextProxy)),
            new Tuple<Type, Type, Type>(typeof(IDataProvider), typeof(IDataProviderProxy), typeof(DataProviderProxy)),
            new Tuple<Type, Type, Type>(typeof(ITranslation), typeof(ITranslationProxy), typeof(TranslationProxy)),
            new Tuple<Type, Type, Type>(typeof(ITranslationInfo), typeof(ITranslationInfoProxy), typeof(TranslationInfoProxy))
        };

        /// <summary>
        /// Private class for testing the basic functionality used by repositories in the food waste domain.
        /// </summary>
        private class MyDataRepository : DataRepositoryBase
        {
            #region Constructor

            /// <summary>
            /// Creates a private class for testing the basic functionality used by repositories in the food waste domain.
            /// </summary>
            /// <param name="foodWasteDataProvider">Implementation of a data provider which can access data in the food waste repository.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            public MyDataRepository(IFoodWasteDataProvider foodWasteDataProvider, IFoodWasteObjectMapper foodWasteObjectMapper) 
                : base(foodWasteDataProvider, foodWasteObjectMapper)
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets the data provider which can access data in the food waste repository.
            /// </summary>
            /// <returns>Data provider which can access data in the food waste repository.</returns>
            public IFoodWasteDataProvider GetDataProvider()
            {
                return DataProvider;
            }

            /// <summary>
            /// Gets the object mapper which can map objects in the food waste domain.
            /// </summary>
            /// <returns>Object mapper which can map objects in the food waste domain.</returns>
            public IFoodWasteObjectMapper GetObjectMapper()
            {
                return ObjectMapper;
            }

            #endregion
        }

        #region Private variables

        private IFoodWasteDataProvider _foodWasteDataProviderMock;
        private IFoodWasteObjectMapper _foodWasteObjectMapperMock;

        #endregion

        /// <summary>
        /// Tests that the constructor initialize the basic functionality used by repositories in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataRepositoryBase()
        {
            MyDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.GetDataProvider(), Is.Not.Null);
            Assert.That(sut.GetDataProvider(), Is.EqualTo(_foodWasteDataProviderMock));
            Assert.That(sut.GetObjectMapper(), Is.Not.Null);
            Assert.That(sut.GetObjectMapper(), Is.EqualTo(_foodWasteObjectMapperMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the data provider which can access data in the food waste repository is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteDataProviderIsNull()
        {
            IFoodWasteObjectMapper foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result  = Assert.Throws<ArgumentNullException>(() => new MyDataRepository(null, foodWasteObjectMapperMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodWasteDataProvider");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteObjectMapperIsNull()
        {
            IFoodWasteDataProvider foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyDataRepository(foodWasteDataProviderMock, null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodWasteObjectMapper");
        }

        /// <summary>
        /// Tests that Get calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatGetCallsGetOnFoodWasteDataProvider()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatGetCallsGetOnFoodWasteDataProvider");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatGetCallsGetOnFoodWasteDataProvider' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item3);
                try
                {
                    genericMethod.Invoke(this, new object[] {Guid.NewGuid()});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Get returns the received data proxy from the food waste repository.
        /// </summary>
        [Test]
        public void TestThatGetReturnsReceivedDataProxy()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatGetReturnsReceivedDataProxy");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatGetReturnsReceivedDataProxy' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item3);
                try
                {
                    genericMethod.Invoke(this, new object[] {Guid.NewGuid()});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item3);
                try
                {
                    genericMethod.Invoke(this, new object[] {Guid.NewGuid()});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item3);
                try
                {
                    genericMethod.Invoke(this, new object[] {Guid.NewGuid()});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Insert throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatInsertThrowsArgumentNullExceptionWhenIdentifiableIsNull()
        {
            MyDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Insert((IIdentifiable) null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "identifiable");
        }

        /// <summary>
        /// Tests that Insert calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatInsertCallsMapOnFoodWasteObjectMapper()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertCallsMapOnFoodWasteObjectMapper");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatInsertCallsMapOnFoodWasteObjectMapper' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Insert calls Add on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatInsertCallsAddOnFoodWasteDataProvider()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertCallsAddOnFoodWasteDataProvider");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatInsertCallsAddOnFoodWasteDataProvider' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Insert returns the inserted data proxy.
        /// </summary>
        [Test]
        public void TestThatInsertReturnsInsertedDataProxy()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertReturnsInsertedDataProxy");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatInsertReturnsInsertedDataProxy' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Update throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatUpdateThrowsArgumentNullExceptionWhenIdentifiableIsNull()
        {
            MyDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Update((IIdentifiable) null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "identifiable");
        }

        /// <summary>
        /// Tests that Update calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatUpdateCallsMapOnFoodWasteObjectMapper()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateCallsMapOnFoodWasteObjectMapper");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatUpdateCallsMapOnFoodWasteObjectMapper' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Update calls Save on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatUpdateCallsSaveOnFoodWasteDataProvider()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateCallsSaveOnFoodWasteDataProvider");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatUpdateCallsSaveOnFoodWasteDataProvider' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Update returns the updated data proxy.
        /// </summary>
        [Test]
        public void TestThatUpdateReturnsUpdatedDataProxy()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateReturnsUpdatedDataProxy");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatUpdateReturnsUpdatedDataProxy' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Delete throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatDeleteThrowsArgumentNullExceptionWhenIdentifiableIsNull()
        {
            MyDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Delete((IIdentifiable) null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "identifiable");
        }

        /// <summary>
        /// Tests that Delete calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatDeleteCallsMapOnFoodWasteObjectMapper()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatDeleteCallsMapOnFoodWasteObjectMapper");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatDeleteCallsMapOnFoodWasteObjectMapper' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Delete calls Delete on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDeleteCallsDeleteOnFoodWasteDataProvider()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatDeleteCallsDeleteOnFoodWasteDataProvider");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatDeleteCallsDeleteOnFoodWasteDataProvider' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            foreach (var typeToTest in TypesToTest)
            {
                MethodInfo method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs");
                if (method == null)
                {
                    throw new Exception($"Can't find a method named 'TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs' on the type named '{GetType().Name}'.");
                }

                MethodInfo genericMethod = method.MakeGenericMethod(typeToTest.Item1, typeToTest.Item2);
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null)
                    {
                        throw;
                    }
                    throw new Exception($"{typeToTest.Item1.Name}: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Creates an instanse of the private class for testing the basic functionality used by repositories in the food waste domain.
        /// </summary>
        /// <returns>Instanse of the private class for testing the basic functionality used by repositories in the food waste domain.</returns>
        private MyDataRepository CreateSut()
        {
            return CreateSutForGettingData<IHousehold, HouseholdProxy>();
        }

        /// <summary>
        /// Creates an instanse of the private class for testing the basic functionality used by repositories in the food waste domain.
        /// </summary>
        /// <returns>Instanse of the private class for testing the basic functionality used by repositories in the food waste domain.</returns>
        private MyDataRepository CreateSutForGettingData<TIdentifiable, TDataProxy>(TDataProxy dataProxy = null, Exception exceptionToThrow = null) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy, new()
        {
            _foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            if (exceptionToThrow != null)
            {
                _foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
            }
            else
            {
                _foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                    .Return(dataProxy ?? new TDataProxy())
                    .Repeat.Any();
            }

            _foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            return new MyDataRepository(_foodWasteDataProviderMock, _foodWasteObjectMapperMock);
        }

        /// <summary>
        /// Creates an instanse of the private class for testing the basic functionality used by repositories in the food waste domain.
        /// </summary>
        /// <returns>Instanse of the private class for testing the basic functionality used by repositories in the food waste domain.</returns>
        private MyDataRepository CreateSutForModifyingData<TIdentifiable, TDataProxy>(TDataProxy dataProxy, Exception exceptionToThrow = null) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        {
            _foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            if (exceptionToThrow != null)
            {
                _foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.Delete(Arg<TDataProxy>.Is.NotNull))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
            }
            else
            {
                _foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                    .Return(dataProxy)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                    .Return(dataProxy)
                    .Repeat.Any();
            }

            _foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            _foodWasteObjectMapperMock.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxy)
                .Repeat.Any();

            return new MyDataRepository(_foodWasteDataProviderMock, _foodWasteObjectMapperMock);
        }

        /// <summary>
        /// Tests that Get calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatGetCallsGetOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy, new()
        // ReSharper restore UnusedMember.Local
        {
            MyDataRepository sut = CreateSutForGettingData<TIdentifiable, TDataProxy>();
            Assert.That(sut, Is.Not.Null);

            sut.Get<TIdentifiable>(identifier);

            _foodWasteDataProviderMock.AssertWasCalled(m => m.Get(Arg<TDataProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tests that Get returns the received data proxy from the food waste repository.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatGetReturnsReceivedDataProxy<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy, new()
        // ReSharper restore UnusedMember.Local
        {
            TDataProxy dataProxy = new TDataProxy();

            MyDataRepository sut = CreateSutForGettingData<TIdentifiable, TDataProxy>(dataProxy: dataProxy);
            Assert.That(sut, Is.Not.Null);

            TIdentifiable result = sut.Get<TIdentifiable>(identifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProxy));
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy, new()
        // ReSharper restore UnusedMember.Local
        {
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

            MyDataRepository sut = CreateSutForGettingData<TIdentifiable, TDataProxy>(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.Get<TIdentifiable>(identifier));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy, new()
        // ReSharper restore UnusedMember.Local
        {
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

            MyDataRepository sut = CreateSutForGettingData<TIdentifiable, TDataProxy>(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.Get<TIdentifiable>(identifier));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "Get", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that Insert calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatInsertCallsMapOnFoodWasteObjectMapper<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock);
            Assert.That(sut, Is.Not.Null);

            sut.Insert(identifiable);

            _foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.Equal(identifiable), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Insert calls Add on the data provider which can access data in the food waste repository.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatInsertCallsAddOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock);
            Assert.That(sut, Is.Not.Null);

            sut.Insert(identifiable);

            _foodWasteDataProviderMock.AssertWasCalled(m => m.Add(Arg<TDataProxy>.Is.Equal(dataProxyMock)));
        }

        /// <summary>
        /// Tests that Insert returns the inserted data proxy.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatInsertReturnsInsertedDataProxy<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock);
            Assert.That(sut, Is.Not.Null);

            sut.Insert(identifiable);

            TIdentifiable result = sut.Insert(identifiable);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProxyMock));
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock, exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.Insert(identifiable));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock, exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.Insert(identifiable));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "Insert", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that Update calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatUpdateCallsMapOnFoodWasteObjectMapper<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock);
            Assert.That(sut, Is.Not.Null);

            sut.Update(identifiable);

            _foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.Equal(identifiable), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Update calls Save on the data provider which can access data in the food waste repository.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatUpdateCallsSaveOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock);
            Assert.That(sut, Is.Not.Null);

            sut.Update(identifiable);

            _foodWasteDataProviderMock.AssertWasCalled(m => m.Save(Arg<TDataProxy>.Is.Equal(dataProxyMock)));
        }

        /// <summary>
        /// Tests that Update returns the updated data proxy.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatUpdateReturnsUpdatedDataProxy<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock);
            Assert.That(sut, Is.Not.Null);

            IIdentifiable result = sut.Update(identifiable);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProxyMock));
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock, exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.Update(identifiable));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock, exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.Update(identifiable));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "Update", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that Delete calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatDeleteCallsMapOnFoodWasteObjectMapper<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock);
            Assert.That(sut, Is.Not.Null);

            sut.Delete(identifiable);

            _foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.Equal(identifiable), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Delete calls Delete on the data provider which can access data in the food waste repository.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatDeleteCallsDeleteOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock);
            Assert.That(sut, Is.Not.Null);

            sut.Delete(identifiable);

            _foodWasteDataProviderMock.AssertWasCalled(m => m.Delete(Arg<TDataProxy>.Is.Equal(dataProxyMock)));
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock, exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.Delete(identifiable));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private void TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy
        // ReSharper restore UnusedMember.Local
        {
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

            MyDataRepository sut = CreateSutForModifyingData<TIdentifiable, TDataProxy>(dataProxyMock, exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.Delete(identifiable));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "Delete", exceptionToThrow.Message);
        }

        /// <summary>
        /// Generates a mockup of a given type.
        /// </summary>
        /// <param name="mockType">Type on wich to generate the mockup.</param>
        /// <returns>Mockup of the given type.</returns>
        private static object GenerateMock(Type mockType)
        {
            if (mockType == null)
            {
                throw new ArgumentNullException(nameof(mockType));
            }

            return typeof(MockRepository).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Single(m => m.IsGenericMethod && m.Name == "GenerateMock" && m.GetGenericArguments().Length == 1)
                .MakeGenericMethod(mockType)
                .Invoke(null, new object[] {null});
        }
    }
}
