using System;

namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation
{
    /// <summary>
    /// Interface for a specification which encapsulates validation rules.
    /// </summary>
    public interface ISpecification
    {
        /// <summary>
        /// Adds a delegate for validation.
        /// </summary>
        /// <param name="candidate">Delegate for validation which returns a boolean.</param>
        /// <param name="exception">Exception which should be thrown when the delegate returns false.</param>
        /// <returns>Specification which encapsulates validation rules.</returns>
        ISpecification IsSatisfiedBy(Func<bool> candidate, Exception exception);

        /// <summary>
        /// Validates all the added delegates for validation and throws an exception when the first validation fails.
        /// </summary>
        void Evaluate();
    }
}
