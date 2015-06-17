using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Data provider.
    /// </summary>
    public class DataProvider : TranslatableBase, IDataProvider
    {
        #region Private variables

        private string _name;
        private Guid _dataSourceStatementIdentifier;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data provider.
        /// </summary>
        protected DataProvider()
        {
        }

        /// <summary>
        /// Creates a data provider.
        /// </summary>
        /// <param name="name">Name for the data provider.</param>
        /// <param name="dataSourceStatementIdentifier">Identifier for the data source statement.</param>
        public DataProvider(string name, Guid dataSourceStatementIdentifier)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            _name = name;
            _dataSourceStatementIdentifier = dataSourceStatementIdentifier;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name for the data provider.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                _name = value;
            }
        }

        /// <summary>
        /// Gets the identifier for the data source statement.
        /// </summary>
        public virtual Guid DataSourceStatementIdentifier
        {
            get
            {
                return _dataSourceStatementIdentifier;
            }
            protected set
            {
                _dataSourceStatementIdentifier = value;
            }
        }

        /// <summary>
        /// Translation for the data source statement.
        /// </summary>
        public ITranslation DataSourceStatement { get; private set; }

        /// <summary>
        /// Gets the translations of the data source statement.
        /// </summary>
        public virtual IEnumerable<ITranslation> DataSourceStatements
        {
            get { return Translations.Where(m => m.TranslationOfIdentifier == DataSourceStatementIdentifier).ToList(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finish up the translation for the data provider.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        protected override void OnTranslation(CultureInfo translationCulture)
        {
            base.OnTranslation(translationCulture);
            DataSourceStatement = Translate(translationCulture, DataSourceStatementIdentifier);
        }

        #endregion
    }
}
