﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Household member.
    /// </summary>
    public class HouseholdMember : IdentifiableBase, IHouseholdMember
    {
        #region Private variables

        private string _mailAddress;
        private Membership _membership;
        private DateTime? _membershipExpireTime;
        private string _activationCode;
        private DateTime _creationTime;
        private IList<IHousehold> _households = new List<IHousehold>(0);
        private readonly IDomainObjectValidations _domainObjectValidations;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a household member.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member.</param>
        /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
        public HouseholdMember(string mailAddress, IDomainObjectValidations domainObjectValidations = null) 
            : this(mailAddress, Membership.Basic, null, GenerateActivationCode(), DateTime.Now, domainObjectValidations)
        {
        }

        /// <summary>
        /// Creates a household member.
        /// </summary>
        protected HouseholdMember()
        {
            _domainObjectValidations = DomainObjectValidations.Create();
        }

        /// <summary>
        /// Creates a household member.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member.</param>
        /// <param name="membership">Membership.</param>
        /// <param name="membershipExpireTime">Date and time for when the membership expires.</param>
        /// <param name="activationCode">Activation code for the household member.</param>
        /// <param name="creationTime">Date and time for when the household member was created.</param>
        /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
        protected HouseholdMember(string mailAddress, Membership membership, DateTime? membershipExpireTime, string activationCode, DateTime creationTime, IDomainObjectValidations domainObjectValidations = null)
        {
            if (string.IsNullOrEmpty(mailAddress))
            {
                throw new ArgumentNullException("mailAddress");
            }
            if (string.IsNullOrEmpty(activationCode))
            {
                throw new ArgumentNullException("activationCode");
            }

            _domainObjectValidations = domainObjectValidations ?? DomainObjectValidations.Create();
            if (_domainObjectValidations.IsMailAddress(mailAddress) == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, mailAddress, "mailAddress"));
            }
            _mailAddress = mailAddress;

            _membership = membership;
            _membershipExpireTime = membershipExpireTime;
            _activationCode = activationCode;
            _creationTime = creationTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Mail address for the household member.
        /// </summary>
        public virtual string MailAddress
        {
            get
            {
                return _mailAddress;
            }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (_domainObjectValidations.IsMailAddress(value) == false)
                {
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value, "value"));
                }
                _mailAddress = value;
            }
        }

        /// <summary>
        /// Membership.
        /// </summary>
        public virtual Membership Membership
        {
            get
            {
                if (MembershipExpireTime.HasValue == false || MembershipExpireTime.Value < DateTime.Now)
                {
                    return Membership.Basic;
                }
                return _membership;
            }
            protected set
            {
                _membership = value;
                if (_membership == Membership.Basic)
                {
                    MembershipExpireTime = null;
                }
            }
        }

        /// <summary>
        /// Date and time for when the membership expires.
        /// </summary>
        public virtual DateTime? MembershipExpireTime
        {
            get { return _membershipExpireTime; }
            protected set { _membershipExpireTime = value; }
        }

        /// <summary>
        /// Activation code for the household member.
        /// </summary>
        public virtual string ActivationCode
        {
            get
            {
                return _activationCode;
            }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                _activationCode = value;
            }
        }

        /// <summary>
        /// Date and time for when the household member was activated.
        /// </summary>
        public virtual DateTime? ActivationTime
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the household member is activated.
        /// </summary>
        public virtual bool IsActivated
        {
            get
            {
                return ActivationTime.HasValue && ActivationTime.Value <= DateTime.Now;
            }
        }

        /// <summary>
        /// Date and time for when the household member has accepted our privacy policy.
        /// </summary>
        public virtual DateTime? PrivacyPolicyAcceptedTime
        {
            get; 
            set;
        }

        /// <summary>
        /// Indicates whether the household member has accepted our privacy policy.
        /// </summary>
        public virtual bool IsPrivacyPolictyAccepted
        {
            get
            {
                return PrivacyPolicyAcceptedTime.HasValue && PrivacyPolicyAcceptedTime.Value <= DateTime.Now;
            }
        }

        /// <summary>
        /// Date and time for when the household member was created.
        /// </summary>
        public virtual DateTime CreationTime
        {
            get { return _creationTime; }
            protected set { _creationTime = value; }
        }

        /// <summary>
        /// Households on which the household member has a membership.
        /// </summary>
        public virtual IEnumerable<IHousehold> Households
        {
            get
            {
                return _households;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value.Count() > _domainObjectValidations.GetHouseholdLimit(Membership))
                {
                    throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdLimitHasBeenReached));
                }
                _households = value.ToList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies a new membership to the household member.
        /// </summary>
        /// <param name="membership">Membership which should be applied to the household member.</param>
        public virtual void MembershipApply(Membership membership)
        {
            switch (membership)
            {
                case Membership.Basic:
                    Membership = membership;
                    MembershipExpireTime = null;
                    break;

                case Membership.Deluxe:
                case Membership.Premium:
                    Membership = membership;
                    MembershipExpireTime = DateTime.Now.AddYears(1);
                    break;

                default:
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, membership, "membership", MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Adds a household to the household member
        /// </summary>
        /// <param name="household">Household on which the household member has a membership.</param>
        public virtual void HouseholdAdd(IHousehold household)
        {
            if (household == null)
            {
                throw new ArgumentNullException("household");
            }
            if (_domainObjectValidations.HasReachedHouseholdLimit(Membership, Households.Count()))
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdLimitHasBeenReached));
            }
            _households.Add(household);
        }

        /// <summary>
        /// Generates an activation code.
        /// </summary>
        /// <returns>Activation code.</returns>
        private static string GenerateActivationCode()
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("D")));

                var activationCodeBuilder = new StringBuilder();
                for (var i = 0; i < hash.Length; i++)
                {
                    activationCodeBuilder.Append(hash[i].ToString("X2"));
                }
                return activationCodeBuilder.ToString();
            }
        }

        #endregion
    }
}
