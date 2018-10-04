namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies
{
    /// <summary>
    /// Interface for a builder which can build a database command tester.
    /// </summary>
    internal interface IDbCommandTestBuilder
    {
        /// <summary>
        /// Adds a smallint data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="size">The size for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddSmallIntDataParameter(string parameterName, int? value, int size, bool isNullable = false);

        /// <summary>
        /// Adds a varchar data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="size">The size for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddVarCharDataParameter(string parameterName, string value, int size, bool isNullable = false);

        /// <summary>
        /// Build the executor which can test a database command.
        /// </summary>
        /// <returns>Executor which can test a database command.</returns>
        IDbCommandTestExecutor Build();
    }
}
