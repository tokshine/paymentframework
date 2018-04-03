using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workwiz.PaymentFramework.Shared.CapitaApi;
using Workwiz.PaymentFramework.Shared.CapitaSimple;

namespace Workwiz.PaymentFramework.Tests.CapitaApi
{
    /// <summary>
    /// Summary description for CapitaApiHelperTests
    /// </summary>
    [TestClass]
    public class CapitaApiHelperTests
    {
       
        [TestMethod]
        public void GetCredentialsToHash_CredentialObject_ConcatenatedString()
        {
            credentials capitalCredentials = new credentials()
            {
                subject = new subject()
                {
                    subjectType = subjectType.CapitaPortal,
                    identifier = 174064579
                },
                requestIdentification = new requestIdentification()
                {
                    uniqueReference = "123456",
                    timeStamp = "20170131125459",

                },
                signature = new signature()
                {
                    algorithm = algorithm.Original,
                    hmacKeyID = 456

                }
            };

            string stringToHash = CapitaApiHelpers.GetCredentialsToHash(capitalCredentials);
            Assert.AreEqual(stringToHash, "CapitaPortal!174064579!123456!20170131125459!Original!456");

        }

        [TestMethod]
        public void CalculateDigest_ValidInput_HashString()
        {
            credentials capitalCredentials = new credentials()
            {
                subject = new subject()
                {
                    subjectType = subjectType.CapitaPortal,
                    identifier = 174064579
                },
                requestIdentification = new requestIdentification()
                {
                    uniqueReference = "123456",
                    timeStamp = "20170131125459",

                },
                signature = new signature()
                {
                    algorithm = algorithm.Original,
                    hmacKeyID = 456

                }
            };

            string hmacKey =
                "zgtQwyBsiFkL7ioGpH9YqiYioYpbkQjMmkBrvA6IXGBmzwx+Q5tFn6qbgVgKl95oIiPPHYpWaLquNRWXesBP3w==";

            string stringToHash = CapitaApiHelpers.GetCredentialsToHash(capitalCredentials);
            string hash = CapitaApiHelpers.CalculateDigest(hmacKey, stringToHash);

            Assert.AreEqual(hash, "X+MlsmdD5RxMQ6/yPaQ0wzJY146oMD0Sp4g3hbXweTU=");

        }
    }
}
