using System;
using System.Collections.Generic;
using Castle.Core.Internal;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;

namespace OSDevGrp.OSIntranet.Infrastructure.Validation
{
    /// <summary>
    /// Specification which encapsulates validation rules.
    /// </summary>
    public class Specification : ISpecification
    {
        #region Private variables

        private readonly IList<Action> _candidates = new List<Action>();

        #endregion

        #region Methods

        /// <summary>
        /// Adds a delegate for validation.
        /// </summary>
        /// <param name="candidate">Delegate for validation which returns a boolean.</param>
        /// <param name="exception">Exception which should be thrown when the delegate returns false.</param>
        /// <returns>Specification which encapsulates validation rules.</returns>
        public virtual ISpecification IsSatisfiedBy(Func<bool> candidate, Exception exception)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException("candidate");
            }
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            Action evaluationAction = () =>
            {
                if (candidate())
                {
                    return;
                }
                throw exception;
            };
            _candidates.Add(evaluationAction);
            return this;
        }

        /// <summary>
        /// Validates all the added delegates for validation and throws an exception when the first validation fails.
        /// </summary>
        public virtual void Evaluate()
        {
            _candidates.ForEach(m => m.Invoke());
        }

        #endregion
    }
}
