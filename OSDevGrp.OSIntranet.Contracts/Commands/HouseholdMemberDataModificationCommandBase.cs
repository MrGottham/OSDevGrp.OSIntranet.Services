using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for modifying some data on a household member.
    /// </summary>
    [DataContract(Name = "HouseholdMemberDataModificationCommandBase", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public abstract class HouseholdMemberDataModificationCommandBase : ICommand
    {
    }
}
