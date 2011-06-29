using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en hjælper på en regnskabsliste.
    /// </summary>
    public interface IRegnskabslisteHelper : IDomainObjectListHelper<Regnskab, int>
    {
    }
}
