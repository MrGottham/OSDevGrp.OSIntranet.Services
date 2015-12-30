namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface to a provider which can resolve values from the current users claims.
    /// </summary>
    public interface IClaimValueProvider
    {
        /// <summary>
        /// Gets the mail address from the current users email claim.
        /// </summary>
        string MailAddress { get; }
    }
}
