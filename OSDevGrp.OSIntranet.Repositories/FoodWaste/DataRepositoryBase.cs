using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.FoodWaste
{
    /// <summary>
    /// Basic functionality used by repositories in the food waste domain.
    /// </summary>
    public abstract class DataRepositoryBase : IDataRepository
    {
        #region Private variables

        private readonly IFoodWasteDataProvider _foodWasteDataProvider;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates basic functionality used by repositories in the food waste domain.
        /// </summary>
        /// <param name="foodWasteDataProvider">Implementation of a data provider which can access data in the food waste repository.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        protected DataRepositoryBase(IFoodWasteDataProvider foodWasteDataProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
        {
            _foodWasteDataProvider = foodWasteDataProvider ?? throw new ArgumentNullException(nameof(foodWasteDataProvider));
            _foodWasteObjectMapper = foodWasteObjectMapper ?? throw new ArgumentNullException(nameof(foodWasteObjectMapper));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Data provider which can access data in the food waste repository.
        /// </summary>
        protected virtual IFoodWasteDataProvider DataProvider => _foodWasteDataProvider;

        /// <summary>
        /// Object mapper which can map objects in the food waste domain.
        /// </summary>
        protected virtual IFoodWasteObjectMapper ObjectMapper => _foodWasteObjectMapper;

        #endregion

        #region Methods

        /// <summary>
        /// Gets a given identifiable domain object from the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <param name="identifier">Identifier for the domain object to get.</param>
        /// <returns>The identifiable domain object.</returns>
        public virtual TIdentifiable Get<TIdentifiable>(Guid identifier) where TIdentifiable : IIdentifiable
        {
            if (typeof(TIdentifiable) == typeof(IHousehold))
            {
                return (TIdentifiable) Get<IHousehold, HouseholdProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(IStorageType))
            {
                return (TIdentifiable) Get<IStorageType, StorageTypeProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(IHouseholdMember))
            {
                return (TIdentifiable) Get<IHouseholdMember, HouseholdMemberProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(IPayment))
            {
                return (TIdentifiable) Get<IPayment, PaymentProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(IFoodItem))
            {
                return (TIdentifiable) Get<IFoodItem, FoodItemProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(IFoodGroup))
            {
                return (TIdentifiable) Get<IFoodGroup, FoodGroupProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(IForeignKey))
            {
                return (TIdentifiable) Get<IForeignKey, ForeignKeyProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(IStaticText))
            {
                return (TIdentifiable) Get<IStaticText, StaticTextProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(IDataProvider))
            {
                return (TIdentifiable) Get<IDataProvider, DataProviderProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(ITranslation))
            {
                return (TIdentifiable) Get<ITranslation, TranslationProxy>(identifier);
            }
            if (typeof(TIdentifiable) == typeof(ITranslationInfo))
            {
                return (TIdentifiable) Get<ITranslationInfo, TranslationInfoProxy>(identifier);
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Inserts an identifiable domain object in the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <param name="identifiable">Identifiable domain object to insert.</param>
        /// <returns>The inserted identifiable domain object.</returns>
        public virtual TIdentifiable Insert<TIdentifiable>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable
        {
            if (Equals(identifiable, null))
            {
                throw new ArgumentNullException(nameof(identifiable));
            }

            IHousehold household = identifiable as IHousehold;
            if (household != null)
            {
                return (TIdentifiable) Insert<IHousehold, IHouseholdProxy>(household);
            }

            IStorageType storageType = identifiable as IStorageType;
            if (storageType != null)
            {
                return (TIdentifiable) Insert<IStorageType, IStorageTypeProxy>(storageType);
            }

            IHouseholdMember householdMember = identifiable as IHouseholdMember;
            if (householdMember != null)
            {
                return (TIdentifiable) Insert<IHouseholdMember, IHouseholdMemberProxy>(householdMember);
            }

            IPayment payment = identifiable as IPayment;
            if (payment != null)
            {
                return (TIdentifiable) Insert<IPayment, IPaymentProxy>(payment);
            }

            IFoodItem foodItem = identifiable as IFoodItem;
            if (foodItem != null)
            {
                return (TIdentifiable) Insert<IFoodItem, IFoodItemProxy>(foodItem);
            }

            IFoodGroup foodGroup = identifiable as IFoodGroup;
            if (foodGroup != null)
            {
                return (TIdentifiable) Insert<IFoodGroup, IFoodGroupProxy>(foodGroup);
            }

            IForeignKey foreignKey = identifiable as IForeignKey;
            if (foreignKey != null)
            {
                return (TIdentifiable) Insert<IForeignKey, IForeignKeyProxy>(foreignKey);
            }

            IStaticText staticText = identifiable as IStaticText;
            if (staticText != null)
            {
                return (TIdentifiable) Insert<IStaticText, IStaticTextProxy>(staticText);
            }

            IDataProvider dataProvider = identifiable as IDataProvider;
            if (dataProvider != null)
            {
                return (TIdentifiable) Insert<IDataProvider, IDataProviderProxy>(dataProvider);
            }

            ITranslation translation = identifiable as ITranslation;
            if (translation != null)
            {
                return (TIdentifiable) Insert<ITranslation, ITranslationProxy>(translation);
            }

            ITranslationInfo translationInfo = identifiable as ITranslationInfo;
            if (translationInfo != null)
            {
                return (TIdentifiable) Insert<ITranslationInfo, ITranslationInfoProxy>(translationInfo);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Updates an identifiable domain object in the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <param name="identifiable">Identifiable domain object to update.</param>
        /// <returns>The updated identifiable domain object.</returns>
        public virtual TIdentifiable Update<TIdentifiable>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable
        {
            if (Equals(identifiable, null))
            {
                throw new ArgumentNullException(nameof(identifiable));
            }

            IHousehold household = identifiable as IHousehold;
            if (household != null)
            {
                return (TIdentifiable) Update<IHousehold, IHouseholdProxy>(household);
            }

            IStorageType storageType = identifiable as IStorageType;
            if (storageType != null)
            {
                return (TIdentifiable) Update<IStorageType, IStorageTypeProxy>(storageType);
            }

            IHouseholdMember householdMember = identifiable as IHouseholdMember;
            if (householdMember != null)
            {
                return (TIdentifiable) Update<IHouseholdMember, IHouseholdMemberProxy>(householdMember);
            }

            IPayment payment = identifiable as IPayment;
            if (payment != null)
            {
                return (TIdentifiable) Update<IPayment, IPaymentProxy>(payment);
            }

            IFoodItem foodItem = identifiable as IFoodItem;
            if (foodItem != null)
            {
                return (TIdentifiable) Update<IFoodItem, IFoodItemProxy>(foodItem);
            }

            IFoodGroup foodGroup = identifiable as IFoodGroup;
            if (foodGroup != null)
            {
                return (TIdentifiable) Update<IFoodGroup, IFoodGroupProxy>(foodGroup);
            }

            IForeignKey foreignKey = identifiable as IForeignKey;
            if (foreignKey != null)
            {
                return (TIdentifiable) Update<IForeignKey, IForeignKeyProxy>(foreignKey);
            }

            IStaticText staticText = identifiable as IStaticText;
            if (staticText != null)
            {
                return (TIdentifiable) Update<IStaticText, IStaticTextProxy>(staticText);
            }

            IDataProvider dataProvider = identifiable as IDataProvider;
            if (dataProvider != null)
            {
                return (TIdentifiable) Update<IDataProvider, IDataProviderProxy>(dataProvider);
            }

            ITranslation translation = identifiable as ITranslation;
            if (translation != null)
            {
                return (TIdentifiable) Update<ITranslation, ITranslationProxy>(translation);
            }

            ITranslationInfo translationInfo = identifiable as ITranslationInfo;
            if (translationInfo != null)
            {
                return (TIdentifiable) Update<ITranslationInfo, ITranslationInfoProxy>(translationInfo);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Deletes an identifiable domain object in the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <param name="identifiable">Identifiable domain object to delete.</param>
        public void Delete<TIdentifiable>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable
        {
            if (Equals(identifiable, null))
            {
                throw new ArgumentNullException(nameof(identifiable));
            }

            IHousehold household = identifiable as IHousehold;
            if (household != null)
            {
                Delete<IHousehold, IHouseholdProxy>(household);
                return;
            }

            IStorageType storageType = identifiable as IStorageType;
            if (storageType != null)
            {
                Delete<IStorageType, IStorageTypeProxy>(storageType);
                return;
            }

            IHouseholdMember householdMember = identifiable as IHouseholdMember;
            if (householdMember != null)
            {
                Delete<IHouseholdMember, IHouseholdMemberProxy>(householdMember);
                return;
            }

            IPayment payment = identifiable as IPayment;
            if (payment != null)
            {
                Delete<IPayment, IPaymentProxy>(payment);
                return;
            }

            IFoodItem foodItem = identifiable as IFoodItem;
            if (foodItem != null)
            {
                Delete<IFoodItem, IFoodItemProxy>(foodItem);
                return;
            }

            IFoodGroup foodGroup = identifiable as IFoodGroup;
            if (foodGroup != null)
            {
                Delete<IFoodGroup, IFoodGroupProxy>(foodGroup);
                return;
            }

            IForeignKey foreignKey = identifiable as IForeignKey;
            if (foreignKey != null)
            {
                Delete<IForeignKey, IForeignKeyProxy>(foreignKey);
                return;
            }

            IStaticText staticText = identifiable as IStaticText;
            if (staticText != null)
            {
                Delete<IStaticText, IStaticTextProxy>(staticText);
                return;
            }

            IDataProvider dataProvider = identifiable as IDataProvider;
            if (dataProvider != null)
            {
                Delete<IDataProvider, IDataProviderProxy>(dataProvider);
                return;
            }

            ITranslation translation = identifiable as ITranslation;
            if (translation != null)
            {
                Delete<ITranslation, ITranslationProxy>(translation);
                return;
            }

            ITranslationInfo translationInfo = identifiable as ITranslationInfo;
            if (translationInfo != null)
            {
                Delete<ITranslationInfo, ITranslationInfoProxy>(translationInfo);
                return;
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a given identifiable domain object from the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <typeparam name="TDataProxy">Type of the data proxy for the identifiable domain object.</typeparam>
        /// <param name="identifier">Identifier for the domain object to get.</param>
        /// <returns>The identifiable domain object.</returns>
        protected virtual TIdentifiable Get<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>, new()
        {
            try
            {
                var dataProxy = new TDataProxy
                {
                    Identifier = identifier
                };
                return _foodWasteDataProvider.Get(dataProxy);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Get", ex.Message), ex);
            }
        }

        /// <summary>
        /// Inserts an identifiable domain object in the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <typeparam name="TDataProxy">Type of the data proxy for the identifiable domain object.</typeparam>
        /// <param name="identifiable">Identifiable domain object to insert.</param>
        /// <returns>The inserted identifiable domain object.</returns>
        protected virtual TIdentifiable Insert<TIdentifiable, TDataProxy>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            if (Equals(identifiable, null))
            {
                throw new ArgumentNullException(nameof(identifiable));
            }
            try
            {
                if (identifiable.Identifier.HasValue == false)
                {
                    identifiable.Identifier = Guid.NewGuid();
                }
                var dataProxy = _foodWasteObjectMapper.Map<TIdentifiable, TDataProxy>(identifiable);
                return _foodWasteDataProvider.Add(dataProxy);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Insert", ex.Message), ex);
            }
        }

        /// <summary>
        /// Updates an identifiable domain object in the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <typeparam name="TDataProxy">Type of the data proxy for the identifiable domain object.</typeparam>
        /// <param name="identifiable">Identifiable domain object to update.</param>
        /// <returns>The updated identifiable domain object.</returns>
        protected virtual TIdentifiable Update<TIdentifiable, TDataProxy>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            if (Equals(identifiable, null))
            {
                throw new ArgumentNullException(nameof(identifiable));
            }
            try
            {
                var dataProxy = _foodWasteObjectMapper.Map<TIdentifiable, TDataProxy>(identifiable);
                return _foodWasteDataProvider.Save(dataProxy);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Update", ex.Message), ex);
            }
        }

        /// <summary>
        /// Deletes an identifiable domain object in the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <typeparam name="TDataProxy">Type of the data proxy for the identifiable domain object.</typeparam>
        /// <param name="identifiable">Identifiable domain object to delete.</param>
        protected virtual void Delete<TIdentifiable, TDataProxy>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            if (Equals(identifiable, null))
            {
                throw new ArgumentNullException(nameof(identifiable));
            }
            try
            {
                var dataProxy = _foodWasteObjectMapper.Map<TIdentifiable, TDataProxy>(identifiable);
                _foodWasteDataProvider.Delete(dataProxy);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Delete", ex.Message), ex);
            }
        }

        #endregion
    }
}
