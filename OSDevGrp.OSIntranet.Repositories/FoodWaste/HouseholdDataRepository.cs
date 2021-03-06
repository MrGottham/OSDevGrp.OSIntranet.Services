﻿using System;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.FoodWaste
{
    /// <summary>
    /// Repository which can access household data for the food waste domain.
    /// </summary>
    public class HouseholdDataRepository : DataRepositoryBase, IHouseholdDataRepository
    {
        #region Constructor

        /// <summary>
        /// Creates a repository which can access household data for the food waste domain.
        /// </summary>
        /// <param name="foodWasteDataProvider">Implementation of a data provider which can access data in the food waste repository.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        public HouseholdDataRepository(IFoodWasteDataProvider foodWasteDataProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
            : base(foodWasteDataProvider, foodWasteObjectMapper)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a household member by their mail address.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member to get.</param>
        /// <returns>Household member when exists; otherwise null.</returns>
        public virtual IHouseholdMember HouseholdMemberGetByMailAddress(string mailAddress)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(mailAddress, nameof(mailAddress));

            try
            {
                MySqlCommand command = HouseholdMemberProxy.BuildHouseholdDataCommandForSelecting("WHERE MailAddress=@mailAddress", householdDataCommandBuilder => householdDataCommandBuilder.AddMailAddressParameter(mailAddress));
                return DataProvider.GetCollection<HouseholdMemberProxy>(command).SingleOrDefault();
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

        #endregion
    }
}
