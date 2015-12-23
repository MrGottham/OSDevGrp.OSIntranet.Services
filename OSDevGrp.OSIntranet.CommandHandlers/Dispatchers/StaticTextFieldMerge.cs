using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Dispatchers
{
    /// <summary>
    /// Functionality which can merge fields in a static text.
    /// </summary>
    public class StaticTextFieldMerge : IStaticTextFieldMerge
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IDictionary<StaticTextType, IStaticText> _staticTextCache = new Dictionary<StaticTextType, IStaticText>();

        private readonly IDictionary<string, object> _mergeFields = new Dictionary<string, object>
        {
            {"[PrivacyPoliciesSubject]", new Lazy<string>(() => string.Empty)},
            {"[PrivacyPoliciesBody]", new Lazy<string>(() => string.Empty)}
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the functionality which can merge fields in a static text.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        public StaticTextFieldMerge(ISystemDataRepository systemDataRepository)
        {
            if (systemDataRepository == null)
            {
                throw new ArgumentNullException("systemDataRepository");
            }
            _systemDataRepository = systemDataRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the fields and values which can be merged.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<string, object>> MergeFields
        {
            get { return _mergeFields; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Merge field values into the static text.
        /// </summary>
        /// <param name="staticText">Static text on which to merge field values.</param>
        /// <param name="translationInfo">Translation informations used to translate the static text.</param>
        public void Merge(IStaticText staticText, ITranslationInfo translationInfo)
        {
            if (staticText == null)
            {
                throw new ArgumentNullException("staticText");
            }
            if (translationInfo == null)
            {
                throw new ArgumentNullException("translationInfo");
            }

            staticText.Translate(translationInfo.CultureInfo);

            if (staticText.SubjectTranslation == null && staticText.BodyTranslation == null)
            {
                return;
            }

            if (staticText.SubjectTranslation != null && string.IsNullOrEmpty(staticText.SubjectTranslation.Value) == false)
            {
                staticText.SubjectTranslation.Value = Merge(staticText.SubjectTranslation.Value, translationInfo);
            }

            if (staticText.BodyTranslation != null && string.IsNullOrEmpty(staticText.BodyTranslation.Value) == false)
            {
                staticText.BodyTranslation.Value = Merge(staticText.BodyTranslation.Value, translationInfo);
            }
        }

        /// <summary>
        /// Merge field values into the given value.
        /// </summary>
        /// <param name="value">Value on which to merge field values.</param>
        /// <param name="translationInfo">Translation informations used to translate the static text.</param>
        /// <returns>Value with merged field values.</returns>
        private string Merge(string value, ITranslationInfo translationInfo)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            if (translationInfo == null)
            {
                throw new ArgumentNullException("translationInfo");
            }

            _mergeFields["[PrivacyPoliciesSubject]"] = new Lazy<string>(() => ResolveStaticText(StaticTextType.PrivacyPolicy, translationInfo).SubjectTranslation.Value);
            _mergeFields["[PrivacyPoliciesBody]"] = new Lazy<string>(() => ResolveStaticText(StaticTextType.PrivacyPolicy, translationInfo).BodyTranslation.Value);

            var valueBuilder = new StringBuilder(value);
            foreach (var mergeField in _mergeFields.Where(m => value.Contains(m.Key)))
            {
                var lazyValue = mergeField.Value as Lazy<string>;
                if (lazyValue != null)
                {
                    valueBuilder.Replace(mergeField.Key, lazyValue.Value);
                    continue;
                }
                
                valueBuilder.Replace(mergeField.Key, Convert.ToString(mergeField.Value));
            }
            return valueBuilder.ToString();
        }

        /// <summary>
        /// Resolves the static text for a given static text type.
        /// </summary>
        /// <param name="staticTextType">Static text type for which to resolve the static text.</param>
        /// <param name="translationInfo">Translation informations used to translate the static text.</param>
        /// <returns>Static text for a given static text type.</returns>
        private IStaticText ResolveStaticText(StaticTextType staticTextType, ITranslationInfo translationInfo)
        {
            if (translationInfo == null)
            {
                throw new ArgumentNullException("translationInfo");
            }

            IStaticText staticText;
            if (_staticTextCache.ContainsKey(staticTextType))
            {
                staticText = _staticTextCache[staticTextType];
                staticText.Translate(translationInfo.CultureInfo);
                return staticText;
            }

            staticText = _systemDataRepository.StaticTextGetByStaticTextType(staticTextType);
            foreach (var translation in staticText.Translations)
            {
                translation.Value = translation.Value.Replace("<html>", string.Empty).Replace("</html>", string.Empty);
            }
            staticText.Translate(translationInfo.CultureInfo);

            _staticTextCache.Add(staticTextType, staticText);
            return staticText;
        }

        #endregion
    }
}
