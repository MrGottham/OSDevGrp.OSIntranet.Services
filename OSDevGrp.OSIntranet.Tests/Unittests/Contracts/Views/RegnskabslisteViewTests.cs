﻿using System;
using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject en regnskabsliste.
    /// </summary>
    [TestFixture]
    public class RegnskabslisteViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new RegnskabslisteView
                           {
                               Number = 1,
                               Navn = "Privatregnskab"
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.Number, Is.EqualTo(1));
            Assert.That(view.Navn, Is.Not.Null);
            Assert.That(view.Navn, Is.EqualTo("Privatregnskab"));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new RegnskabslisteView
                           {
                               Number = 1,
                               Navn = "Privatregnskab"
                           };
            Assert.That(view, Is.Not.Null);
            var memoryStream = new MemoryStream();
            try
            {
                var serializer = new DataContractSerializer(view.GetType());
                serializer.WriteObject(memoryStream, view);
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