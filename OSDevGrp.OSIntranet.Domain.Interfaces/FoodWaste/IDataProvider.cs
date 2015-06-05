using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a data provider.
    /// </summary>
    public interface IDataProvider : IIdentifiable
    {
        /// <summary>
        /// Gets the name for the data provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the identifier for the data source statement.
        /// </summary>
        Guid DataSourceStatementIdentifier { get; }

        /// <summary>
        /// Gets the translations of the data source statement.
        /// </summary>
        IEnumerable<ITranslation> DataSourceStatements { get; }
    }
}
