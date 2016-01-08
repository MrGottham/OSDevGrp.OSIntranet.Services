using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for common validations used by domain objects in the food waste domain.
    /// </summary>
    public interface IDomainObjectValidations
    {
        /// <summary>
        /// Validates whether a value is a mail address.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if the value is a mail address otherwise false.</returns>
        bool IsMailAddress(string value);

        /// <summary>
        /// Gets the limit of households according to a given membership.
        /// </summary>
        /// <param name="membership">Membership.</param>
        /// <returns>Limit of households according to a given membership.</returns>
        int GetHouseholdLimit(Membership membership);

        /// <summary>
        /// Validates whether the limit of households has been reached accoing to a given membesrhip.
        /// </summary>
        /// <param name="membership">Membership.</param>
        /// <param name="numberOfHouseholds">Number of households.</param>
        /// <returns>True if the limit of households has been reached otherwise false.</returns>
        bool HasReachedHouseholdLimit(Membership membership, int numberOfHouseholds);

        /// <summary>
        /// Validates whether a given membership matches the required membership.
        /// </summary>
        /// <param name="membership">Membership which should match the required membership.</param>
        /// <param name="requiredMembership">Required membership.</param>
        /// <returns>True if the given membership matches the required membership otherwise false.</returns>
        bool HasRequiredMembership(Membership membership, Membership requiredMembership);
    }
}
