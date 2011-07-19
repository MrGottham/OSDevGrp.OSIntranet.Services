using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts
{
    /// <summary>
    /// Hjælpeklasse til test at datakontrakter.
    /// </summary>
    public static class DataContractTestHelper
    {
        /// <summary>
        /// Tester, at en datakontrakt er initieret.
        /// </summary>
        /// <typeparam name="TContract">Typen på datakontrakten.</typeparam>
        /// <param name="contract">Datakontrakt.</param>
        public static void TestAtContractErInitieret<TContract>(TContract contract)
        {
            Assert.That(contract, Is.Not.Null);

            var properties = contract.GetType().GetProperties();
            foreach (var property in properties)
            {
                Assert.That(property.GetValue(contract, null), Is.Not.Null);
            }
        }

        /// <summary>
        /// Tester, at en datakontrakt kan serialiseres og deserialiseres.
        /// </summary>
        /// <typeparam name="TContract">Typen på datakontrakten.</typeparam>
        /// <param name="contract">Datakontrakt.</param>
        public static void TestAtContractKanSerialiseresOgDeserialiseres<TContract>(TContract contract)
        {
            Assert.That(contract, Is.Not.Null);

            using (var memoryStream = new MemoryStream())
            {
                // Serialisér datakontrakt.
                var serialiser = new DataContractSerializer(typeof(TContract));
                serialiser.WriteObject(memoryStream, contract);
                memoryStream.Flush();
                Assert.That(memoryStream.Length, Is.GreaterThan(0));

                // Deserialisér datakontrakt.
                memoryStream.Seek(0, SeekOrigin.Begin);
                var result = (TContract) serialiser.ReadObject(memoryStream);
                Assert.That(result, Is.Not.Null);

                // Test deserialiseret resultat.
                CompareContracts(contract, result);

                memoryStream.Close();
            }
        }

        /// <summary>
        /// Sammenligner værdier i to objekter.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="target">Target.</param>
        private static void CompareContracts(object source, object target)
        {
            Assert.That(source, Is.Not.Null);
            Assert.That(target, Is.Not.Null);

            var properties = source.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(source, null);

                if (property.PropertyType.IsValueType)
                {
                    Assert.That(value, Is.EqualTo(target.GetType().GetProperty(property.Name).GetValue(target, null)));
                    continue;
                }
                if (value is System.String)
                {
                    Assert.That(value, Is.EqualTo(target.GetType().GetProperty(property.Name).GetValue(target, null)));
                    continue;
                }
                if (property.PropertyType.IsClass)
                {
                    CompareContracts(value, target.GetType().GetProperty(property.Name).GetValue(target, null));
                    continue;
                }

                Assert.Fail(string.Format("Can't compare value for the type: {0}", property.PropertyType));
            }
        }
    }
}
