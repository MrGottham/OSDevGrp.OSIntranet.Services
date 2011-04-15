using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Responses
{
    /// <summary>
    /// Tester datakontrakt for en bogføringsadvarsel.
    /// </summary>
    [TestFixture]
    public class BogføringsadvarselResponseTests
    {
        /// <summary>
        /// Tester, at Response kan initieres.
        /// </summary>
        [Test]
        public void TestAtResponseKanInitieres()
        {
            var response = new BogføringsadvarselResponse
                               {
                                   Advarsel = "Budgettet på budgetkontoen er overtrukket.",
                                   Konto = new BudgetkontoView(),
                                   Beløb = 1500M
                               };
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Advarsel, Is.Not.Null);
            Assert.That(response.Advarsel, Is.EqualTo("Budgettet på budgetkontoen er overtrukket."));
            Assert.That(response.Konto, Is.Not.Null);
            Assert.That(response.Beløb, Is.EqualTo(1500M));
        }

        /// <summary>
        /// Tester, at Response kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtResponseKanSerialiseres()
        {
            var response = new BogføringsadvarselResponse
                               {
                                   Advarsel = "Budgettet på budgetkontoen er overtrukket.",
                                   Konto = new BudgetkontoView(),
                                   Beløb = 1500M
                               };
            Assert.That(response, Is.Not.Null);
            var memoryStream = new MemoryStream();
            try
            {
                var serializer = new DataContractSerializer(response.GetType());
                serializer.WriteObject(memoryStream, response);
                memoryStream.Flush();
                Assert.That(memoryStream.Length, Is.GreaterThan(0));
            }
            finally
            {
                memoryStream.Close();
            }
        }
    }
}
