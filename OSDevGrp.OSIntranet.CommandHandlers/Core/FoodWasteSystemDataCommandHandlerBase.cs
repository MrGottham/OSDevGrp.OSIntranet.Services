using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basic functionality for command handlers which handles commands for system data in the food waste domain.
    /// </summary>
    public abstract class FoodWasteSystemDataCommandHandlerBase : CommandHandlerTransactionalBase
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;
        private readonly ISpecification _specification;
        private readonly ICommonValidations _commonValidations;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the basic functionality for command handlers which handles commands for system data in the food waste domain.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        protected FoodWasteSystemDataCommandHandlerBase(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations)
        {
            if (systemDataRepository == null)
            {
                throw new ArgumentNullException("systemDataRepository");
            }
            if (foodWasteObjectMapper == null)
            {
                throw new ArgumentNullException("foodWasteObjectMapper");
            }
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }
            if (commonValidations == null)
            {
                throw new ArgumentNullException("commonValidations");
            }
            _systemDataRepository = systemDataRepository;
            _foodWasteObjectMapper = foodWasteObjectMapper;
            _specification = specification;
            _commonValidations = commonValidations;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the repository which can access system data for the food waste domain.
        /// </summary>
        protected virtual ISystemDataRepository SystemDataRepository
        {
            get { return _systemDataRepository; }
        }

        /// <summary>
        /// Gets the object mapper which can map objects in the food waste domain.
        /// </summary>
        protected virtual IFoodWasteObjectMapper ObjectMapper
        {
            get { return _foodWasteObjectMapper; }
        }

        /// <summary>
        /// Gets the specification which encapsulates validation rules.
        /// </summary>
        protected virtual ISpecification Specification
        {
            get { return _specification; }
        }

        /// <summary>
        /// Gets the common validations.
        /// </summary>
        protected virtual ICommonValidations CommonValidations
        {
            get { return _commonValidations; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Imports a given translation on a given translatable domain object.
        /// </summary>
        /// <param name="domainObject">Translatable domain object on which to import the translation.</param>
        /// <param name="translationInfo">Translation informations for the translation to import.</param>
        /// <param name="translationValue">The translation value for the translatable domain object.</param>
        /// <param name="logicExecutor">Implementation of the logic executor which can execute basic logic.</param>
        /// <returns>The imported translation.</returns>
        protected virtual ITranslation ImportTranslation(ITranslatable domainObject, ITranslationInfo translationInfo, string translationValue, ILogicExecutor logicExecutor)
        {
            if (domainObject == null)
            {
                throw new ArgumentNullException("domainObject");
            }
            if (translationInfo == null)
            {
                throw new ArgumentNullException("translationInfo");
            }
            if (string.IsNullOrEmpty(translationValue))
            {
                throw new ArgumentNullException("translationValue");
            }
            if (logicExecutor == null)
            {
                throw new ArgumentNullException("logicExecutor");
            }
            var domainObjectIdentifier = domainObject.Identifier.HasValue ? domainObject.Identifier.Value : default(Guid);
            var translationInfoIdentifier = translationInfo.Identifier.HasValue ? translationInfo.Identifier.Value : default(Guid);
            var translation = domainObject.Translations.SingleOrDefault(m => m.TranslationOfIdentifier == domainObjectIdentifier && m.TranslationInfo.Identifier.HasValue && m.TranslationInfo.Identifier.Value == translationInfoIdentifier);
            if (translation == null)
            {
                var insertedTranslation = new Translation(domainObjectIdentifier, translationInfo, translationValue);
                insertedTranslation.Identifier = logicExecutor.TranslationAdd(insertedTranslation);
                domainObject.TranslationAdd(insertedTranslation);
                return insertedTranslation;
            }
            translation.Value = translationValue;
            translation.Identifier = logicExecutor.TranslationModify(translation);
            return translation;
        }

        #endregion
    }
}
