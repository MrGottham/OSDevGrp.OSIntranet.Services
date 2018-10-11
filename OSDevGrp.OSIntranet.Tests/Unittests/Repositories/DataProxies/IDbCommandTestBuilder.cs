using System;

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
        /// Adds an int data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="size">The size for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddIntDataParameter(string parameterName, int? value, int size, bool isNullable = false);

        /// <summary>
        /// Adds an bit data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddBitDataParameter(string parameterName, bool? value, bool isNullable = false);

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
        /// Adds a char data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="size">The size for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddCharDataParameter(string parameterName, string value, int size, bool isNullable = false);

        /// <summary>
        /// Adds a char data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddCharDataParameter(string parameterName, Guid value, bool isNullable = false);

        /// <summary>
        /// Adds a text data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddTextDataParameter(string parameterName, string value, bool isNullable = false);

        /// <summary>
        /// Adds a date data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddDateParameter(string parameterName, DateTime? value, bool isNullable = false);

        /// <summary>
        /// Adds a time data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddTimeParameter(string parameterName, DateTime? value, bool isNullable = false);

        /// <summary>
        /// Adds a time data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        IDbCommandTestBuilder AddTimeParameter(string parameterName, TimeSpan? value, bool isNullable = false);

        /// <summary>
        /// Build the executor which can test a database command.
        /// </summary>
        /// <returns>Executor which can test a database command.</returns>
        IDbCommandTestExecutor Build();
    }
}
