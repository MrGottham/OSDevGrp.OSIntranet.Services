﻿using MySql.Data.MySqlClient;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders
{
    /// <summary>
    /// Interface til en data provider, som benytter MySql.
    /// </summary>
    public interface IMySqlDataProvider : IDataProviderBase<MySqlDataReader, MySqlCommand>
    {
    }
}
