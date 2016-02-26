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
using Ploeh.AutoFixture;
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
            new Tuple<Type, Type, Type>(typeof (IHousehold), typeof (IHouseholdProxy), typeof (HouseholdProxy)),
            new Tuple<Type, Type, Type>(typeof (IHouseholdMember), typeof (IHouseholdMemberProxy), typeof (HouseholdMemberProxy)),
            new Tuple<Type, Type, Type>(typeof (IPayment), typeof (IPaymentProxy), typeof (PaymentProxy)),
            new Tuple<Type, Type, Type>(typeof (IFoodItem), typeof (IFoodItemProxy), typeof (FoodItemProxy)),
            new Tuple<Type, Type, Type>(typeof (IFoodGroup), typeof (IFoodGroupProxy), typeof (FoodGroupProxy)),
            new Tuple<Type, Type, Type>(typeof (IForeignKey), typeof (IForeignKeyProxy), typeof (ForeignKeyProxy)),
            new Tuple<Type, Type, Type>(typeof (IStaticText), typeof (IStaticTextProxy), typeof (StaticTextProxy)),
            new Tuple<Type, Type, Type>(typeof (IDataProvider), typeof (IDataProviderProxy), typeof (DataProviderProxy)),
            new Tuple<Type, Type, Type>(typeof (ITranslation), typeof (ITranslationProxy), typeof (TranslationProxy)),
            new Tuple<Type, Type, Type>(typeof (ITranslationInfo), typeof (ITranslationInfoProxy), typeof (TranslationInfoProxy))
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

        /// <summary>
        /// Tests that the constructor initialize the basic functionality used by repositories in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataRepositoryBase()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);
            Assert.That(dataRepositoryBase.GetDataProvider(), Is.Not.Null);
            Assert.That(dataRepositoryBase.GetDataProvider(), Is.EqualTo(foodWasteDataProviderMock));
            Assert.That(dataRepositoryBase.GetObjectMapper(), Is.Not.Null);
            Assert.That(dataRepositoryBase.GetObjectMapper(), Is.EqualTo(foodWasteObjectMapper));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the data provider which can access data in the food waste repository is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteDataProviderIsNull()
        {
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyDataRepository(null, foodWasteObjectMapperMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteDataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteObjectMapperIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyDataRepository(foodWasteDataProviderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Get calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatGetCallsGetOnFoodWasteDataProvider()
        {
            foreach (var typeToTest in TypesToTest)
            {
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatGetCallsGetOnFoodWasteDataProvider");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatGetCallsGetOnFoodWasteDataProvider' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item3});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item3.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new object[] {Guid.NewGuid()});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatGetReturnsReceivedDataProxy");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatGetReturnsReceivedDataProxy' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item3});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item3.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new object[] {Guid.NewGuid()});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item3});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item3.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new object[] {Guid.NewGuid()});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item3});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item3.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new object[] {Guid.NewGuid()});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
                }
            }
        }

        /// <summary>
        /// Tests that Insert throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatInsertThrowsArgumentNullExceptionWhenIdentifiableIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataRepositoryBase.Insert((IIdentifiable) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identifiable"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Insert calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatInsertCallsMapOnFoodWasteObjectMapper()
        {
            foreach (var typeToTest in TypesToTest)
            {
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertCallsMapOnFoodWasteObjectMapper");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatInsertCallsMapOnFoodWasteObjectMapper' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertCallsAddOnFoodWasteDataProvider");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatInsertCallsAddOnFoodWasteDataProvider' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertReturnsInsertedDataProxy");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatInsertReturnsInsertedDataProxy' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
                }
            }
        }

        /// <summary>
        /// Tests that Update throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatUpdateThrowsArgumentNullExceptionWhenIdentifiableIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataRepositoryBase.Update((IIdentifiable) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identifiable"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Update calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatUpdateCallsMapOnFoodWasteObjectMapper()
        {
            foreach (var typeToTest in TypesToTest)
            {
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateCallsMapOnFoodWasteObjectMapper");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatUpdateCallsMapOnFoodWasteObjectMapper' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateCallsSaveOnFoodWasteDataProvider");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatUpdateCallsSaveOnFoodWasteDataProvider' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateReturnsUpdatedDataProxy");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatUpdateReturnsUpdatedDataProxy' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
                }
            }
        }

        /// <summary>
        /// Tests that Delete throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatDeleteThrowsArgumentNullExceptionWhenIdentifiableIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataRepositoryBase.Delete((IIdentifiable) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identifiable"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Delete calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatDeleteCallsMapOnFoodWasteObjectMapper()
        {
            foreach (var typeToTest in TypesToTest)
            {
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatDeleteCallsMapOnFoodWasteObjectMapper");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatDeleteCallsMapOnFoodWasteObjectMapper' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatDeleteCallsDeleteOnFoodWasteDataProvider");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatDeleteCallsDeleteOnFoodWasteDataProvider' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
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
                var method = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic).SingleOrDefault(m => m.IsGenericMethod && m.Name == "TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs");
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find a method named 'TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs' on the type named '{0}'.", GetType().Name));
                }
                var genericMethod = method.MakeGenericMethod(new[] {typeToTest.Item1, typeToTest.Item2});
                if (genericMethod == null)
                {
                    throw new Exception(string.Format("Can't make the generic method of '{0}' with the types named '{1}' and '{2}'.", method.Name, typeToTest.Item1.Name, typeToTest.Item2.Name));
                }
                try
                {
                    genericMethod.Invoke(this, new[] {GenerateMock(typeToTest.Item1), GenerateMock(typeToTest.Item2)});
                }
                catch (TargetInvocationException ex)
                {
                    throw new Exception(string.Format("{0}: {1}", typeToTest.Item1.Name, ex.InnerException.Message));
                }
            }
        }

        /// <summary>
        /// Tests that Get calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        private static void TestThatGetCallsGetOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>, new()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                .Return(new TDataProxy())
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Get<TIdentifiable>(identifier);

            foodWasteDataProviderMock.AssertWasCalled(m => m.Get(Arg<TDataProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tests that Get returns the received data proxy from the food waste repository.
        /// </summary>
        private static void TestThatGetReturnsReceivedDataProxy<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>, new()
        {
            var dataProxy = new TDataProxy();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxy)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var result = dataRepositoryBase.Get<TIdentifiable>(identifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProxy));
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        private static void TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>, new()
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Get<TIdentifiable>(identifier));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        private static void TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>, new()
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Get<TIdentifiable>(identifier));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Get", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Insert calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        private static void TestThatInsertCallsMapOnFoodWasteObjectMapper<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Insert(identifiable);

            foodWasteObjectMapper.AssertWasCalled(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.Equal(identifiable), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Insert calls Add on the data provider which can access data in the food waste repository.
        /// </summary>
        private static void TestThatInsertCallsAddOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Insert(identifiable);

            foodWasteDataProviderMock.AssertWasCalled(m => m.Add(Arg<TDataProxy>.Is.Equal(dataProxyMock)));
        }

        /// <summary>
        /// Tests that Insert returns the inserted data proxy.
        /// </summary>
        private static void TestThatInsertReturnsInsertedDataProxy<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var result = dataRepositoryBase.Insert(identifiable);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProxyMock));
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        private static void TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Insert(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        private static void TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Insert(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Insert", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Update calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        private static void TestThatUpdateCallsMapOnFoodWasteObjectMapper<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Update(identifiable);

            foodWasteObjectMapper.AssertWasCalled(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.Equal(identifiable), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Update calls Save on the data provider which can access data in the food waste repository.
        /// </summary>
        private static void TestThatUpdateCallsSaveOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Update(identifiable);

            foodWasteDataProviderMock.AssertWasCalled(m => m.Save(Arg<TDataProxy>.Is.Equal(dataProxyMock)));
        }

        /// <summary>
        /// Tests that Update returns the updated data proxy.
        /// </summary>
        private static void TestThatUpdateReturnsUpdatedDataProxy<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var result = dataRepositoryBase.Update(identifiable);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProxyMock));
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        private static void TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Update(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        private static void TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Update(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Update", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Delete calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        private static void TestThatDeleteCallsMapOnFoodWasteObjectMapper<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Delete(identifiable);

            foodWasteObjectMapper.AssertWasCalled(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.Equal(identifiable), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Delete calls Delete on the data provider which can access data in the food waste repository.
        /// </summary>
        private static void TestThatDeleteCallsDeleteOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Delete(identifiable);

            foodWasteDataProviderMock.AssertWasCalled(m => m.Delete(Arg<TDataProxy>.Is.Equal(dataProxyMock)));
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        private static void TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Delete(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Delete(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        private static void TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Delete(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Delete(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Delete", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
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
                throw new ArgumentNullException("mockType");
            }
            return typeof (MockRepository).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Single(m => m.IsGenericMethod && m.Name == "GenerateMock" && m.GetGenericArguments().Length == 1)
                .MakeGenericMethod(new[] {mockType})
                .Invoke(null, new object[] {null});
        }
    }
}
