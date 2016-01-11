using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for accepting privacy policy on the current users household member account.
    /// </summary>
    [DataContract(Name = "HouseholdMemberAcceptPrivacyPolicyCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberAcceptPrivacyPolicyCommand : HouseholdMemberDataModificationCommandBase
    {
    }
}
