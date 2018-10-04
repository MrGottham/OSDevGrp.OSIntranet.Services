using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies
{
    /// <summary>
    /// Executor which can test a database command.
    /// </summary>
    internal class DbCommandTestExecutor : IDbCommandTestExecutor
    {
        #region Private constants

        private const string ParameterRegex = @"\@[\w\.\$]+";
        
        #endregion

        #region Private variables

        private readonly IDbCommand _expected;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an executor which can test a database command.
        /// </summary>
        /// <param name="expected">The expected database command.</param>
        public DbCommandTestExecutor(IDbCommand expected)
        {
            ArgumentNullGuard.NotNull(expected, nameof(expected));

            _expected = expected;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Run the test of the database command.
        /// </summary>
        /// <param name="actual">The actual database command to test.</param>
        /// <returns>True when test has been executed successful; otherwise false.</returns>
        public bool Run(IDbCommand actual)
        {
            ArgumentNullGuard.NotNull(actual, nameof(actual));

            using (actual)
            {
                StringBuilder resultBuilder = new StringBuilder();

                CommandTester(_expected, actual, resultBuilder);

                bool headerWritten = false;
                DataParameterCollectionAgainstCommandTextTester(actual.Parameters, actual.CommandText, resultBuilder, ref headerWritten);
                DataParameterCollectionTester(_expected.Parameters, actual.Parameters, resultBuilder, ref headerWritten);

                if (resultBuilder.Length > 0)
                {
                    Assert.Fail(resultBuilder.ToString());
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Tests the database command.
        /// </summary>
        /// <param name="expected">The expected database command.</param>
        /// <param name="actual">The actual database command.</param>
        /// <param name="resultBuilder">The result builder.</param>
        private static void CommandTester(IDbCommand expected, IDbCommand actual, StringBuilder resultBuilder)
        {
            ArgumentNullGuard.NotNull(expected, nameof(expected))
                .NotNull(actual, nameof(actual))
                .NotNull(resultBuilder, nameof(resultBuilder));

            if (string.CompareOrdinal(expected.CommandText, actual.CommandText) != 0)
            {
                resultBuilder.AppendLine("CommandText does not match:");
                resultBuilder.AppendLine($"- Expected:\t{expected.CommandText}");
                resultBuilder.AppendLine($"- But was:\t{actual.CommandText}");
            }

            if (expected.CommandType != actual.CommandType)
            {
                resultBuilder.AppendLine("CommandType does not match:");
                resultBuilder.AppendLine($"- Expected:\t{expected.CommandType}");
                resultBuilder.AppendLine($"- But was:\t{actual.CommandType}");
            }

            if (expected.CommandTimeout != actual.CommandTimeout)
            {
                resultBuilder.AppendLine("CommandTimeout does not match:");
                resultBuilder.AppendLine($"- Expected:\t{expected.CommandTimeout}");
                resultBuilder.AppendLine($"- But was:\t{actual.CommandTimeout}");
            }
        }

        /// <summary>
        /// Tests the data parameter collection against the command text.
        /// </summary>
        /// <param name="actual">The actual parameter collection.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="resultBuilder">The result builder.</param>
        /// <param name="headerWritten">Indicates whether the header line has been written.</param>
        private static void DataParameterCollectionAgainstCommandTextTester(IDataParameterCollection actual, string commandText, StringBuilder resultBuilder, ref bool headerWritten)
        {
            ArgumentNullGuard.NotNull(actual, nameof(actual))
                .NotNullOrWhiteSpace(commandText, nameof(commandText))
                .NotNull(resultBuilder, nameof(resultBuilder));

            Regex parameterRegex = new Regex(ParameterRegex, RegexOptions.Compiled);
            MatchCollection matchCollection = parameterRegex.Matches(commandText);

            if (matchCollection.Count != actual.Count)
            {
                string ParameterTextWriter(int value) => value == 1 ? "parameter" : "parameters";

                headerWritten = WriteHeaderForDataParameterCollection(resultBuilder, headerWritten);
                resultBuilder.AppendLine($"- Expected:\t{matchCollection.Count} {ParameterTextWriter(matchCollection.Count)}");
                resultBuilder.AppendLine($"- But was:\t{actual.Count} {ParameterTextWriter(actual.Count)}");
            }

            foreach (Match match in matchCollection)
            {
                if (actual.Contains(match.Value))
                {
                    continue;
                }

                headerWritten = WriteHeaderForDataParameterCollection(resultBuilder, headerWritten);
                resultBuilder.AppendLine($"- {match.Value} does not exist in Parameters");
            }
        }

        /// <summary>
        /// Tests the data parameter collection.
        /// </summary>
        /// <param name="expected">The expected data parameter collection.</param>
        /// <param name="actual">The actual parameter collection.</param>
        /// <param name="resultBuilder">The result builder.</param>
        /// <param name="headerWritten">Indicates whether the header line has been written.</param>
        private static void DataParameterCollectionTester(IDataParameterCollection expected, IDataParameterCollection actual, StringBuilder resultBuilder, ref bool headerWritten)
        {
            ArgumentNullGuard.NotNull(expected, nameof(expected))
                .NotNull(actual, nameof(actual))
                .NotNull(resultBuilder, nameof(resultBuilder));

            foreach (IDbDataParameter actualDataParameter in actual)
            {
                string actualDataParameterAsString = ToString(actualDataParameter);

                IDbDataParameter expectedDataParameter = (IDbDataParameter) expected[actualDataParameter.ParameterName];
                if (expectedDataParameter == null)
                {
                    headerWritten = WriteHeaderForDataParameterCollection(resultBuilder, headerWritten);
                    resultBuilder.AppendLine("Expected:\t{null}");
                    resultBuilder.AppendLine($"But was:\t{actualDataParameterAsString}");
                    continue;
                }

                string expectedDataParameterAsString = ToString(expectedDataParameter);
                if (string.CompareOrdinal(expectedDataParameterAsString, actualDataParameterAsString) == 0)
                {
                    continue;
                }

                headerWritten = WriteHeaderForDataParameterCollection(resultBuilder, headerWritten);
                resultBuilder.AppendLine($"Expected:\t{expectedDataParameterAsString}");
                resultBuilder.AppendLine($"But was:\t{actualDataParameterAsString}");
            }
        }

        /// <summary>
        /// Writes the header information for the data parameter collection.
        /// </summary>
        /// <param name="resultBuilder">The result builder.</param>
        /// <param name="headerWritten">Indicates whether the header line has been written.</param>
        /// <returns>True when the header information for the data parameter collection has been written; otherwise false.</returns>
        private static bool WriteHeaderForDataParameterCollection(StringBuilder resultBuilder, bool headerWritten)
        {
            ArgumentNullGuard.NotNull(resultBuilder, nameof(resultBuilder));

            if (headerWritten)
            {
                return true;
            }

            resultBuilder.AppendLine("Parameters does not match:");
            return true;
        }

        /// <summary>
        /// Returns a string which describe the data parameter.
        /// </summary>
        /// <param name="dataParameter">The data parameter.</param>
        /// <returns>String which describe the data parameter.</returns>
        private static string ToString(IDbDataParameter dataParameter)
        {
            ArgumentNullGuard.NotNull(dataParameter, nameof(dataParameter));

            return $"{dataParameter.ParameterName}|Value={dataParameter.Value}|DbType={dataParameter.DbType}|Size={dataParameter.Size}|IsNullable={dataParameter.IsNullable}";
        }

        #endregion
    }
}
