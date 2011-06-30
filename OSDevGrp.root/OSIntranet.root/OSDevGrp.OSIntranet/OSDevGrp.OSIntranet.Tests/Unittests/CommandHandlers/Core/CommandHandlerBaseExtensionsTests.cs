using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester extensions til basisklassen CommandHandlerBase.
    /// </summary>
    [TestFixture]
    public class CommandHandlerBaseExtensionsTests
    {
        /// <summary>
        /// Egen klasse til test af extensions til basisklassen CommandHandlerBase.
        /// </summary>
        private class MyCommandHandle : CommandHandlerBase
        {
        }
    }
}
