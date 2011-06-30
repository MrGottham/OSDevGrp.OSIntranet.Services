using OSDevGrp.OSIntranet.CommandHandlers.Core;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en CommandHandler til finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringCommandHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en CommandHandler til finansstyring.
        /// </summary>
        private class MyCommandHandler : FinansstyringCommandHandlerBase
        {
            /// <summary>
            /// Dannere egen klasse til test af basisklasse for en CommandHandler til finansstyring.
            /// </summary>
            /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyCommandHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
                : base(finansstyringRepository, objectMapper)
            {
            }
        }
    }
}
