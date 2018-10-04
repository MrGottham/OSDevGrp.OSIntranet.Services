using System.Data;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies
{
    /// <summary>
    /// Interface for an executor which can test a database command.
    /// </summary>
    internal interface IDbCommandTestExecutor
    {
        /// <summary>
        /// Run the test of the database command.
        /// </summary>
        /// <param name="actual">The actual database command to test.</param>
        /// <returns>True when test has been executed successful; otherwise false.</returns>
        bool Run(IDbCommand actual);
    }
}
