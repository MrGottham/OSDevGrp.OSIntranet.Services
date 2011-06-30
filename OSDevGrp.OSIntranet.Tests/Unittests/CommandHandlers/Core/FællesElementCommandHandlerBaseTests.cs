using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
    /// </summary>
    [TestFixture]
    public class FællesElementCommandHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
        /// </summary>
        private class MyCommandHandler : FællesElementCommandHandlerBase
        {
            /// <summary>
            /// Danner egen klasse til test af basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
            /// </summary>
            /// <param name="fællesRepository">Implementering af repository til finansstyring.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyCommandHandler(IFællesRepository fællesRepository, IObjectMapper objectMapper)
                : base(fællesRepository, objectMapper)
            {
            }
        }
    }
}
