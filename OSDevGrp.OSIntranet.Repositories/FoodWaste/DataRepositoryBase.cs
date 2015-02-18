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
            if (foodWasteDataProvider == null)
            {
                throw new ArgumentNullException("foodWasteDataProvider");
            }
            if (foodWasteObjectMapper == null)
            {
                throw new ArgumentNullException("foodWasteObjectMapper");
            }
            _foodWasteDataProvider = foodWasteDataProvider;
            _foodWasteObjectMapper = foodWasteObjectMapper;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Data provider which can access data in the food waste repository.
        /// </summary>
        protected virtual IFoodWasteDataProvider DataProvider
        {
            get { return _foodWasteDataProvider; }
        }

        /// <summary>
        /// Object mapper which can map objects in the food waste domain.
        /// </summary>
        protected virtual IFoodWasteObjectMapper ObjectMapper
        {
            get { return _foodWasteObjectMapper; }
        }

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
            if (typeof (TIdentifiable) == typeof (ITranslation))
            {
                return (TIdentifiable) Get<ITranslation, TranslationProxy>(identifier);
            }
            if (typeof (TIdentifiable) == typeof (ITranslationInfo))
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
                throw new ArgumentNullException("identifiable");
            }
            if (identifiable is ITranslation)
            {
                return (TIdentifiable) Insert<ITranslation, ITranslationProxy>(identifiable as ITranslation);
            }
            if (identifiable is ITranslationInfo)
            {
                return (TIdentifiable) Insert<ITranslationInfo, ITranslationInfoProxy>(identifiable as ITranslationInfo);
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
                throw new ArgumentNullException("identifiable");
            }
            if (identifiable is ITranslation)
            {
                return (TIdentifiable) Update<ITranslation, ITranslationProxy>(identifiable as ITranslation);
            }
            if (identifiable is ITranslationInfo)
            {
                return (TIdentifiable) Update<ITranslationInfo, ITranslationInfoProxy>(identifiable as ITranslationInfo);
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
                throw new ArgumentNullException("identifiable");
            }
            if (identifiable is ITranslation)
            {
                Delete<ITranslation, ITranslationProxy>(identifiable as ITranslation);
                return;
            }
            if (identifiable is ITranslationInfo)
            {
                Delete<ITranslationInfo, ITranslationInfoProxy>(identifiable as ITranslationInfo);
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
        protected virtual TIdentifiable Insert<TIdentifiable, TDataProxy>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable where TDataProxy : TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            if (Equals(identifiable, null))
            {
                throw new ArgumentNullException("identifiable");
            }
            try
            {
                var dataProxy = _foodWasteObjectMapper.Map<TIdentifiable, TDataProxy>(identifiable);
                if (dataProxy.Identifier.HasValue == false)
                {
                    dataProxy.Identifier = Guid.NewGuid();
                }
                return (TIdentifiable) _foodWasteDataProvider.Add<IMySqlDataProxy<TIdentifiable>>(dataProxy);
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
        protected virtual TIdentifiable Update<TIdentifiable, TDataProxy>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable where TDataProxy : TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            if (Equals(identifiable, null))
            {
                throw new ArgumentNullException("identifiable");
            }
            try
            {
                var dataProxy = _foodWasteObjectMapper.Map<TIdentifiable, TDataProxy>(identifiable);
                return (TIdentifiable) _foodWasteDataProvider.Save<IMySqlDataProxy<TIdentifiable>>(dataProxy);
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
        protected virtual void Delete<TIdentifiable, TDataProxy>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable where TDataProxy : TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            if (Equals(identifiable, null))
            {
                throw new ArgumentNullException("identifiable");
            }
            try
            {
                var dataProxy = _foodWasteObjectMapper.Map<TIdentifiable, TDataProxy>(identifiable);
                _foodWasteDataProvider.Delete<IMySqlDataProxy<TIdentifiable>>(dataProxy);
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
