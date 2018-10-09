using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender
{
    /// <summary>
    /// Interface til en data proxy for en kalenderbruger under OSWEBDB.
    /// </summary>
    public interface IBrugerProxy : IBruger, IMySqlDataProxy, IMySqlDataProxyCreator<IBrugerProxy>
    {
    }
}
