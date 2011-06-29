using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en hjælper på en liste af grupper til budgetkonti.
    /// </summary>
    public interface IBudgetkontogruppelisteHelper : IDomainObjectListHelper<Budgetkontogruppe, int>
    {
    }
}
