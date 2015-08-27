using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Contracts.Commands;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services
{
    /// <summary>
    /// Helper for integrationstests of services.
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Gets the food groups to import.
        /// </summary>
        public static IEnumerable<FoodGroupImportFromDataProviderCommand> FoodGroupImportFromDataProviderCommands
        {
            get
            {
                var dataProviderIdentifier = new Guid("{5A1B9283-6406-44DF-91C5-F2FB83CC9A42}");
                var daDkTranslationInfoIdentifier = new Guid("{978C7318-AD0A-459C-BEE0-1803A94F50D7}");
                var enUsTranslationInfoIdentifier = new Guid("807E904D-FDF9-418D-9745-B73821B8D07A");
                return new List<FoodGroupImportFromDataProviderCommand>
                {
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "01",
                        Name = "Mælk og mælkeprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "02",
                        Name = "Ost og osteprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "03",
                        Name = "Konsumis",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "04",
                        Name = "Korn og stivelsesprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "05",
                        Name = "Grøntsager og grøntsagsprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "06",
                        Name = "Frugt og frugtprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "07",
                        Name = "Kød og kødprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "08",
                        Name = "Fisk og fiskeprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "09",
                        Name = "Fjerkræ og fjerkræprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "10",
                        Name = "Æg og ægprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "11",
                        Name = "Fedtstoffer og fedtstoprodukter",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "12",
                        Name = "Sukker, honning og sukkervarer",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "13",
                        Name = "Drikkevarer",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "14",
                        Name = "Krydderier og andre hjælpestoffer",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "15",
                        Name = "Sammensatte fødevarer",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "16",
                        Name = "Fødevarer til særlig ernæring",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = "17",
                        Name = "Andre fødevarer",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "01",
                        Name = "Milk and milk products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "02",
                        Name = "Cheese and cheese products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "03",
                        Name = "Ice cream, fruit ice and other edible ices",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "04",
                        Name = "Cereals and cereal products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "05",
                        Name = "Vegetables and vegetable products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "06",
                        Name = "Fruit and fruit products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "07",
                        Name = "Meat and meat products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "08",
                        Name = "Fish and fish products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "09",
                        Name = "Poultry and poultry products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "10",
                        Name = "Egg and egg products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "11",
                        Name = "Fats, oils and their products",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "12",
                        Name = "Sugar, honey and products thereof",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "13",
                        Name = "Beverages",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "14",
                        Name = "Spices and other ingredients",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "15",
                        Name = "Mixed foods",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "16",
                        Name = "Food for special nutritional use",
                        IsActive = true
                    },
                    new FoodGroupImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = "17",
                        Name = "Other foods",
                        IsActive = true
                    }
                };
            }
        }
    }
}
