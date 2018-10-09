using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Fælles
{
    /// <summary>
    /// Interface til en data proxy for et system under OSWEBDB.
    /// </summary>
    public interface ISystemProxy : ISystem, IMySqlDataProxy, IMySqlDataProxyCreator<ISystemProxy>
    {
        /// <summary>
        /// Egenskaber for systemet.
        /// </summary>
        int Properties { get; }
    }
}
