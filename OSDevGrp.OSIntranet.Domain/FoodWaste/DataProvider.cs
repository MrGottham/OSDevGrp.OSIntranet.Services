using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Data provider.
    /// </summary>
    public class DataProvider : IdentifiableBase, IDataProvider
    {
        #region Private variables

        private string _name;
        private Guid _dataSourceStatementIdentifier;
        private IEnumerable<ITranslation> _dataSourceStatements = new List<ITranslation>(0); 

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
        /// Gets the translations of the data source statement.
        /// </summary>
        public virtual IEnumerable<ITranslation> DataSourceStatements
        {
            get
            {
                return _dataSourceStatements;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _dataSourceStatements = value;
            }
        }

        #endregion
    }
}
