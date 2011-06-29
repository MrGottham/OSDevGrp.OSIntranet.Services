using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en hjælper på en liste af kontogrupper.
    /// </summary>
    public interface IKontogruppelisteHelper : IDomainObjectListHelper<Kontogruppe, int>
    {
    }
}
