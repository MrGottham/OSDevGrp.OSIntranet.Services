using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Resources;

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

        /// <summary>
        /// Gets the food items to import.
        /// </summary>
        /// <param name="foodGroupTree">Tree for the food groups.</param>
        /// <param name="translationInfoCollection">Collection of translation informations.</param>
        /// <returns>Commands for food items to import.</returns>
        public static IEnumerable<FoodItemImportFromDataProviderCommand> GetFoodItemImportFromDataProviderCommands(FoodGroupTreeSystemView foodGroupTree, ICollection<TranslationInfoSystemView> translationInfoCollection)
        {
            if (foodGroupTree == null)
            {
                throw new ArgumentNullException(nameof(foodGroupTree));
            }
            if (translationInfoCollection == null)
            {
                throw new ArgumentNullException(nameof(translationInfoCollection));
            }

            var dataProviderIdentifier = foodGroupTree.DataProvider.DataProviderIdentifier;

            var foodGroupDataProviderKeys = foodGroupTree.FoodGroups
                .SelectMany(foodGroup => foodGroup.ForeignKeys)
                .Where(foreignKey => foreignKey.DataProvider != null && foreignKey.DataProvider.DataProviderIdentifier == dataProviderIdentifier)
                .ToDictionary(foreignKey => foreignKey.ForeignKey, foreignKey => foreignKey.ForeignKeyForIdentifier);

            var daDkTranslationInfoIdentifier = translationInfoCollection.Single(m => m.CultureName == "da-DK").TranslationInfoIdentifier;
            var enUsTranslationInfoIdentifier = translationInfoCollection.Single(m => m.CultureName == "en-US").TranslationInfoIdentifier;

            var foodItems = new Dictionary<string, IDictionary<string, string>>(0);
            using (var resourceStream = GetEmbeddedResourceStream("Integrationstests.Testdata.DKFoodComp701_2009-11-16.txt"))
            {
                using (var streamReader = new StreamReader(resourceStream, Encoding.Default))
                {
                    while (streamReader.EndOfStream == false)
                    {
                        var buffer = streamReader.ReadLine();
                        if (buffer == null || buffer.Length < 2 + 4 + 3)
                        {
                            continue;
                        }

                        var foodItemKey = buffer.Substring(2, 4);
                        if (string.IsNullOrEmpty(foodItemKey))
                        {
                            continue;
                        }

                        IDictionary<string, string> foodItemValues;
                        if (foodItems.TryGetValue(foodItemKey, out foodItemValues) == false)
                        {
                            foodItemValues = new Dictionary<string, string>();
                            foodItems.Add(foodItemKey, foodItemValues);
                        }

                        switch (buffer.Substring(6, 3))
                        {
                            case "DAN":
                                foodItemValues.Add("DAN", CommonValidations.IllegalChars.Aggregate(buffer.Substring(10).Trim(), (current, illegalChar) => current.Replace(Convert.ToString(illegalChar), string.Empty)));
                                break;

                            case "ENG":
                                foodItemValues.Add("ENG", CommonValidations.IllegalChars.Aggregate(buffer.Substring(10).Trim(), (current, illegalChar) => current.Replace(Convert.ToString(illegalChar), string.Empty)));
                                break;

                            case "MGR":
                                foodItemValues.Add("MGR", buffer.Substring(10).Trim());
                                break;
                        }
                    }
                    streamReader.Close();
                }
                resourceStream.Close();
            }

            var commands = new List<FoodItemImportFromDataProviderCommand>();
            foreach (var foodItem in foodItems)
            {
                var foodItemValues = foodItem.Value;
                if (foodItemValues.ContainsKey("MGR") == false || string.IsNullOrEmpty(foodItemValues["MGR"]))
                {
                    continue;
                }
                if (foodGroupDataProviderKeys.ContainsKey(foodItemValues["MGR"]) == false)
                {
                    continue;
                }
                var primaryGroupIdentifier = foodGroupDataProviderKeys[foodItemValues["MGR"]];

                if (foodItemValues.ContainsKey("DAN") && string.IsNullOrEmpty(foodItemValues["DAN"]) == false)
                {
                    var command = new FoodItemImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = daDkTranslationInfoIdentifier,
                        Key = foodItem.Key,
                        Name = foodItemValues["DAN"].Trim(),
                        PrimaryFoodGroupIdentifier = primaryGroupIdentifier,
                        IsActive = true
                    };
                    commands.Add(command);
                }

                if (foodItemValues.ContainsKey("ENG") && string.IsNullOrEmpty(foodItemValues["ENG"]) == false)
                {
                    var command = new FoodItemImportFromDataProviderCommand
                    {
                        DataProviderIdentifier = dataProviderIdentifier,
                        TranslationInfoIdentifier = enUsTranslationInfoIdentifier,
                        Key = foodItem.Key,
                        Name = foodItemValues["ENG"].Trim(),
                        PrimaryFoodGroupIdentifier = primaryGroupIdentifier,
                        IsActive = true
                    };
                    commands.Add(command);
                }
            }
            return commands;
        }

        /// <summary>
        /// Gets byte collection containing a test document.
        /// </summary>
        /// <returns>Byte collection containing a test document.</returns>
        public static IEnumerable<byte> GetTestDocument()
        {
            using (var resourceStream = GetEmbeddedResourceStream("Integrationstests.Testdata.TestDocument.pdf"))
            {
                var testDocument = new byte[resourceStream.Length];

                resourceStream.Read(testDocument, 0, testDocument.Length);
                resourceStream.Close();

                return testDocument;
            }
        }

        /// <summary>
        /// Loads the specified manifest resource stream from this assembly.
        /// </summary>
        /// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
        /// <returns>The manifest resource.</returns>
        private static Stream GetEmbeddedResourceStream(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            var assembly = typeof (TestHelpers).Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{name}");
            if (stream == null)
            {
                throw new FileNotFoundException($"The embedded manifest resource named '{assembly.GetName().Name}.{name}' could not be found.");
            }
            return stream;
        }

        /// <summary>
        /// Assert an expected FaultException where the details is typeof FoodWasteFault.
        /// </summary>
        /// <param name="faultException">The fault exception.</param>
        /// <param name="expectedFaultType">The expected fault type.</param>
        /// <param name="expectedServiceName">The expected service name.</param>
        /// <param name="expectedExceptionMessage">The expected service message.</param>
        /// <param name="expectedServiceMethod">The expected name of the service method.</param>
        /// <param name="expectedArguments">The expected arguments for the exception message.</param>
        public static void AssertFaultExceptionWithFoodWasteFault(FaultException<FoodWasteFault> faultException, FoodWasteFaultType expectedFaultType, string expectedServiceName, string expectedServiceMethod, ExceptionMessage expectedExceptionMessage, params object[] expectedArguments)
        {
            if (string.IsNullOrEmpty(expectedServiceName))
            {
                throw new ArgumentNullException(nameof(expectedServiceName));
            }
            if (string.IsNullOrEmpty(expectedServiceMethod))
            {
                throw new ArgumentNullException(nameof(expectedServiceMethod));
            }

            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);
            Assert.That(faultException.Detail.FaultType, Is.EqualTo(expectedFaultType));
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
            Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
            Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(Resource.GetExceptionMessage(expectedExceptionMessage, expectedArguments)));
            Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceName, Is.EqualTo(expectedServiceName));
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
            Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
            Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo(expectedServiceMethod));
            Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
            Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
        }
    }
}
