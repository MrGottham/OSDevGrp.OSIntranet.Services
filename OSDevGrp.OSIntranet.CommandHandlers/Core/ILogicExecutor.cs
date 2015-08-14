using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Interface for a logic executor which can execute basic logic.
    /// </summary>
    public interface ILogicExecutor
    {
        /// <summary>
        /// Executes functionality which adds a foreign key to a domain object.
        /// </summary>
        /// <param name="foreignKey">Foreign key to add.</param>
        /// <returns>Identifier for the added foreign key.</returns>
        Guid ForeignKeyAdd(IForeignKey foreignKey);

        /// <summary>
        /// Executes functionality which modifies a foreign key for a domain object.
        /// </summary>
        /// <param name="foreignKey">Foreign key to modify.</param>
        /// <returns>Identifier for the modified foreign key.</returns>
        Guid ForeignKeyModify(IForeignKey foreignKey);

        /// <summary>
        /// Executes functionality which delete a foreign key for a domain object.
        /// </summary>
        /// <param name="foreignKey">Foreign key to delete.</param>
        /// <returns>Identifier for the deleted foreign key.</returns>
        Guid ForeignKeyDelete(IForeignKey foreignKey);

        /// <summary>
        /// Execute functionality which adds a translation.
        /// </summary>
        /// <param name="translation">Translation to add.</param>
        /// <returns>Identifier for the added translation.</returns>
        Guid TranslationAdd(ITranslation translation);

        /// <summary>
        /// Execute functionality which modifies a translation.
        /// </summary>
        /// <param name="translation">Translation to modify.</param>
        /// <returns>Identifier for the modified translation.</returns>
        Guid TranslationModify(ITranslation translation);

        /// <summary>
        /// Execute functionality which deletes a translation.
        /// </summary>
        /// <param name="translation">Translation to delete.</param>
        /// <returns>Identifier for the deleted translation.</returns>
        Guid TranslationDelete(ITranslation translation);
    }
}
