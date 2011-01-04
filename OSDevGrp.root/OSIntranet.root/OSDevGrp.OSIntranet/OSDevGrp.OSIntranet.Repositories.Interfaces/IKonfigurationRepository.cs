namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Interface til et konfigurationsrepository.
    /// </summary>
    public interface IKonfigurationRepository
    {
        /// <summary>
        /// Angivelse af, om saldo for debitorer skal være større end 0.
        /// </summary>
        bool DebitorSaldoOverNul
        {
            get;
        }

        /// <summary>
        /// Angielse af, om saldo for kreditorer skal være større end 0.
        /// </summary>
        bool KreditorSaldoOverNul
        {
            get;
        }
    }
}
