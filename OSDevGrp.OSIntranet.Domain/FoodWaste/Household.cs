using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Household.
    /// </summary>
    public class Household : IdentifiableBase, IHousehold
    {
        #region Private variables

        private string _name;
        private string _description;
        private DateTime _creationTime;
        private IList<IHouseholdMember> _householdMembers = new List<IHouseholdMember>(0);
        private IList<IStorage> _storages = new List<IStorage>(0);
        private readonly IDomainObjectValidations _domainObjectValidations;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a household.
        /// </summary>
        /// <param name="name">Name for the household.</param>
        /// <param name="description">Description for the household.</param>
        /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
        public Household(string name, string description = null, IDomainObjectValidations domainObjectValidations = null)
            : this(name, description, DateTime.Now, domainObjectValidations)
        {
        }

        /// <summary>
        /// Creates a household.
        /// </summary>
        protected Household()
        {
            _domainObjectValidations = DomainObjectValidations.Create();
        }

        /// <summary>
        /// Creates a household.
        /// </summary>
        /// <param name="name">Name for the household.</param>
        /// <param name="description">Description for the household.</param>
        /// <param name="creationTime">Date and time for when the household was created.</param>
        /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
        protected Household(string name, string description, DateTime creationTime, IDomainObjectValidations domainObjectValidations = null)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(name, nameof(name));

            _name = name;
            _description = description;
            _creationTime = creationTime;

            _domainObjectValidations = domainObjectValidations ?? DomainObjectValidations.Create();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name for the household.
        /// </summary>
        public virtual string Name
        {
            get => _name;
            set
            {
                ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _name = value;
            }
        }

        /// <summary>
        /// Description for the household.
        /// </summary>
        public virtual string Description
        {
            get => _description;
            set => _description = value;
        }

        /// <summary>
        /// Date and time for when the household was created.
        /// </summary>
        public virtual DateTime CreationTime
        {
            get => _creationTime;
            protected set => _creationTime = value;
        }

        /// <summary>
        /// Household members who is member of this household.
        /// </summary>
        public virtual IEnumerable<IHouseholdMember> HouseholdMembers
        {
            get => _householdMembers;
            protected set
            {
                ArgumentNullGuard.NotNull(value, nameof(value));

                _householdMembers = value.ToList();
            }
        }

        /// <summary>
        /// Storages in this household.
        /// </summary>
        public virtual IEnumerable<IStorage> Storages
        {
            get => _storages;
            protected set
            {
                ArgumentNullGuard.NotNull(value, nameof(value));

                _storages = value.ToList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a household member to this household.
        /// </summary>
        /// <param name="householdMember">Household member which should be member on this household.</param>
        public virtual void HouseholdMemberAdd(IHouseholdMember householdMember)
        {
            ArgumentNullGuard.NotNull(householdMember, nameof(householdMember));

            _householdMembers.Add(householdMember);
            if (householdMember.Households.Contains(this))
            {
                return;
            }

            householdMember.HouseholdAdd(this);
        }

        /// <summary>
        /// Removes a household member from this household.
        /// </summary>
        /// <param name="householdMember">Household member which should be removed as a member of this household.</param>
        /// <returns>Household member who has been removed af member of this household.</returns>
        public virtual IHouseholdMember HouseholdMemberRemove(IHouseholdMember householdMember)
        {
            ArgumentNullGuard.NotNull(householdMember, nameof(householdMember));

            IHouseholdMember householdMemberToRemove = HouseholdMembers.SingleOrDefault(householdMember.Equals);
            if (householdMemberToRemove == null)
            {
                return null;
            }

            _householdMembers.Remove(householdMemberToRemove);
            if (householdMemberToRemove.Households.Contains(this))
            {
                householdMemberToRemove.HouseholdRemove(this);
            }

            return householdMemberToRemove;
        }

        /// <summary>
        /// Adds a storage to this household.
        /// </summary>
        /// <param name="storage">Storage which should be added to this household.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="storage"/> is null.</exception>
        /// <exception cref="IntranetSystemException">Thrown when the <paramref name="storage"/> cannot be added to this household.</exception>
        public virtual void StorageAdd(IStorage storage)
        {
            ArgumentNullGuard.NotNull(storage, nameof(storage));

            if (_domainObjectValidations.CanAddStorage(storage, Storages) == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.OperationNotAllowedOnStorage, MethodBase.GetCurrentMethod().Name));
            }

            _storages.Add(storage);
        }

        /// <summary>
        /// Adds a storage to this household.
        /// </summary>
        /// <param name="storage">Storage which should be added to this household.</param>
        /// <param name="householdMember">Household member who tries to add the storage.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="storage"/> or the <paramref name="householdMember"/> is null.</exception>
        /// <exception cref="IntranetBusinessException">Thrown when the <paramref name="householdMember"/> does not have the required membership to add storages to this household.</exception>
        /// <exception cref="IntranetSystemException">Thrown when the <paramref name="storage"/> cannot be added to this household.</exception>
        public virtual void StorageAdd(IStorage storage, IHouseholdMember householdMember)
        {
            ArgumentNullGuard.NotNull(storage, nameof(storage))
                .NotNull(householdMember, nameof(householdMember));

            if (householdMember.CanCreateStorage == false)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotRequiredMembership));
            }

            StorageAdd(storage);
        }

        /// <summary>
        /// Removes a storage from this household.
        /// </summary>
        /// <param name="storage">Storage which should be removed from this household.</param>
        /// <returns>The removed storage.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="storage"/> is null.</exception>
        /// <exception cref="IntranetSystemException">Thrown when the <paramref name="storage"/> cannot be removed from this household.</exception>
        public virtual IStorage StorageRemove(IStorage storage)
        {
            ArgumentNullGuard.NotNull(storage, nameof(storage));

            IStorage storageToRemove = Storages.SingleOrDefault(storage.Equals);
            if (storageToRemove == null)
            {
                return null;
            }

            if (_domainObjectValidations.CanRemoveStorage(storageToRemove) == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.OperationNotAllowedOnStorage, MethodBase.GetCurrentMethod().Name));
            }

            _storages.Remove(storageToRemove);

            return storageToRemove;
        }

        /// <summary>
        /// Removes a storage from this household.
        /// </summary>
        /// <param name="storage">Storage which should be removed from this household.</param>
        /// <param name="householdMember">Household member who tries to remove the storage.</param>
        /// <returns>The removed storage.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="storage"/> or the <paramref name="householdMember"/> is null.</exception>
        /// <exception cref="IntranetBusinessException">Thrown when the <paramref name="householdMember"/> does not have the required membership to remove storages from this household.</exception>
        /// <exception cref="IntranetSystemException">Thrown when the <paramref name="storage"/> cannot be removed from this household.</exception>
        public virtual IStorage StorageRemove(IStorage storage, IHouseholdMember householdMember)
        {
            ArgumentNullGuard.NotNull(storage, nameof(storage))
                .NotNull(householdMember, nameof(householdMember));

            if (householdMember.CanDeleteStorage == false)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotRequiredMembership));
            }

            return StorageRemove(storage);
        }

        /// <summary>
        /// Make translation for the household.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        /// <param name="translateHouseholdMembers">Indicates whether to make translation for all the household members who has a membership on this household.</param>
        /// <param name="translateStorages">Indicates whether to make translation for all the storages on the household.</param>
        public virtual void Translate(CultureInfo translationCulture, bool translateHouseholdMembers, bool translateStorages = true)
        {
            ArgumentNullGuard.NotNull(translationCulture, nameof(translationCulture));

            if (translateHouseholdMembers)
            {
                foreach (IHouseholdMember householdMember in HouseholdMembers)
                {
                    householdMember.Translate(translationCulture, false);
                }
            }

            if (translateStorages == false)
            {
                return;
            }

            foreach (IStorage storage in Storages)
            {
                storage.Translate(translationCulture, false);
            }
        }

        #endregion
    }
}
