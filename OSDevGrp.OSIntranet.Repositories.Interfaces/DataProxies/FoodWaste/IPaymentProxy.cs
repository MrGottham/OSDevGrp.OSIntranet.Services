using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a payment from a stakeholder.
    /// </summary>
    public interface IPaymentProxy : IPayment, IMySqlDataProxy
    {
    }
}
