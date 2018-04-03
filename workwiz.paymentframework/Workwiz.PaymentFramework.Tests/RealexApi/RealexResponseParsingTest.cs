using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workwiz.PaymentFramework.Shared.RealexApi;
using Workwiz.PaymentFramework.Shared.RealexApi.RealAuth;
using Workwiz.PaymentFramework.Shared.RealexApi.RealVault;

namespace Workwiz.PaymentFramework.Tests.RealexApi
{
    /// <summary>
    ///     Summary description for RealexResponseParsingTest
    /// </summary>
    [TestClass]
    public class RealexResponseParsingTest
    {
        /// <summary>
        /// Parse sample result from successfull call to RealVault "receipt-in"
        /// </summary>
        [TestMethod]
        public void ReceiptInResponseSuccess()
        {
            string rawRealexResponse =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<response timestamp=\"20160701060517\">"
                + "   <merchantid>unittest</merchantid>"
                + "   <account>theheightsrec</account>"
                + "   <orderid>f20c8e3d-31b9-4164-a7a5-f1685665b574</orderid>"
                + "   <authcode>005804</authcode>"
                + "   <result>00</result>"
                + "   <cvnresult>U</cvnresult>"
                + "   <avspostcoderesponse>U</avspostcoderesponse>"
                + "   <avsaddressresponse>U</avsaddressresponse>"
                + "   <batchid>2126483</batchid>"
                + "   <message>AUTH CODE:005804</message>"
                + "   <pasref>14673495159568662</pasref>"
                + "   <timetaken>1</timetaken>"
                + "   <authtimetaken>0</authtimetaken>"
                + "   <cardissuer>"
                + "     <bank>HSBC</bank>"
                + "     <country>UNITED KINGDOM</country>"
                + "     <countrycode>GB</countrycode>"
                + "     <region>EUR</region>"
                + "   </cardissuer>"
                + "   <sha1hash>d2bd3d3f6bd4736d03f285e11f1728cd722fae99</sha1hash>"
                + " </response>";

            ReceiptInResponse parsedResponse = RealAuthResponseParser<ReceiptInResponse>.DeserializeFromString(rawRealexResponse, System.Net.HttpStatusCode.OK, null);

            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual("20160701060517", parsedResponse.Timestamp);
            Assert.AreEqual("unittest", parsedResponse.MerchantId);
            Assert.AreEqual("theheightsrec", parsedResponse.Account);
            Assert.AreEqual("f20c8e3d-31b9-4164-a7a5-f1685665b574", parsedResponse.OrderId);
            Assert.AreEqual("005804", parsedResponse.AuthCode);
            Assert.AreEqual("00", parsedResponse.ResultString);
            Assert.AreEqual(0, parsedResponse.Result);
            Assert.AreEqual("U", parsedResponse.CvnResult);
            Assert.AreEqual("U", parsedResponse.AvsPostcodeResponse);
            Assert.AreEqual("U", parsedResponse.AvsAddressResponse);
            Assert.AreEqual("2126483", parsedResponse.BatchId);
            Assert.AreEqual("AUTH CODE:005804", parsedResponse.Message);
            Assert.AreEqual("14673495159568662", parsedResponse.PasRef);
            Assert.AreEqual("1", parsedResponse.TimeTaken);
            Assert.AreEqual("0", parsedResponse.AuthTimeTaken);
//            Assert.IsNotNull(parsedResponse.CardIssuer, "CardIssuer");
//            Assert.AreEqual("HSBC", parsedResponse.CardIssuer.Bank);
//            Assert.AreEqual("UNITED KINGDOM", parsedResponse.CardIssuer.Country);
//            Assert.AreEqual("GBC", parsedResponse.CardIssuer.CountryCode);
//            Assert.AreEqual("EUR", parsedResponse.CardIssuer.Region);
            Assert.AreEqual("d2bd3d3f6bd4736d03f285e11f1728cd722fae99", parsedResponse.Sha1Hash);

            string secretKey = "test1234";

            string correctExpectedHash = parsedResponse.CalculateExpectedSha1Hash(secretKey);
            Assert.AreEqual(correctExpectedHash, parsedResponse.Sha1Hash, "SHA signature");
            Assert.IsTrue(parsedResponse.IsSha1HashCorrect(secretKey));
            Assert.IsFalse(parsedResponse.IsSha1HashCorrect("wrongkey"), "expected mismatch Sha1Hash if the wrong key is used");
        }

        /// <summary>
        /// parse sample response from RealAuth "query"
        /// </summary>
        [TestMethod]
        public void QueryPaymentSuccess()
        {
            string rawRealexResponse =
                "<response timestamp=\"20160704143954\">"
                + "  <merchantid>energykidzltd</merchantid>"
                + "  <account>alfredsutton</account>"
                + "  <orderid>216e5f63-6f70-4c56-bea5-43c52441a567</orderid>"
                + "  <authcode>094613</authcode>"
                + "  <result>00</result>"
                + "  <cvnresult>M</cvnresult>"
                + "  <avspostcoderesponse>U</avspostcoderesponse>"
                + "  <avsaddressresponse>U</avsaddressresponse>"
                + "  <batchid>2120757</batchid>"
                + "  <message>AUTH CODE:094613</message>"
                + "  <pasref>14667825736645877</pasref>"
                + "  <timetaken>0</timetaken>"
                + "  <authtimetaken>0</authtimetaken>"
                + "  <cardnumber>465858XXXXXX1226</cardnumber>"
                + "  <cardissuer>"
                + "    <bank>BARCLAYS BANK PLC</bank>"
                + "    <country>UNITED KINGDOM</country>"
                + "    <countrycode>GB</countrycode>"
                + "    <region>EUR</region>"
                + "  </cardissuer>"
                + "  <tss>"
                + "    <result></result>"
                + "  </tss>"
                + "  <threedsecure>"
                + "    <cavv></cavv>"
                + "    <eci></eci>"
                + "    <xid></xid>"
                + "  </threedsecure>"
                + "  <sha1hash>99d787a351cd441c6453733ed65c0c52db2885cb</sha1hash>"
                + "</response>";

            QueryPaymentResponse parsedResponse = RealAuthResponseParser<QueryPaymentResponse>.DeserializeFromString(rawRealexResponse, System.Net.HttpStatusCode.OK, null);
            Assert.IsNotNull(parsedResponse);

            Assert.AreEqual("20160704143954", parsedResponse.Timestamp);
            Assert.AreEqual("energykidzltd", parsedResponse.MerchantId);
            Assert.AreEqual("alfredsutton", parsedResponse.Account);
            Assert.AreEqual("216e5f63-6f70-4c56-bea5-43c52441a567", parsedResponse.OrderId);
            Assert.AreEqual("094613", parsedResponse.AuthCode);
            Assert.AreEqual("00", parsedResponse.ResultString);
            Assert.AreEqual(0, parsedResponse.Result);
            Assert.AreEqual("M", parsedResponse.CvnResult);
            Assert.AreEqual("U", parsedResponse.AvsPostcodeResponse);
            Assert.AreEqual("U", parsedResponse.AvsAddressResponse);
            Assert.AreEqual("2120757", parsedResponse.BatchId);
            Assert.AreEqual("AUTH CODE:094613", parsedResponse.Message);
            Assert.AreEqual("14667825736645877", parsedResponse.PasRef);
            Assert.AreEqual("0", parsedResponse.TimeTaken);
            Assert.AreEqual("0", parsedResponse.AuthTimeTaken);
            Assert.AreEqual("465858XXXXXX1226", parsedResponse.CardNumberMasked);
            //            Assert.IsNotNull(parsedResponse.CardIssuer, "CardIssuer");
            //            Assert.AreEqual("HSBC", parsedResponse.CardIssuer.Bank);
            //            Assert.AreEqual("UNITED KINGDOM", parsedResponse.CardIssuer.Country);
            //            Assert.AreEqual("GBC", parsedResponse.CardIssuer.CountryCode);
            //            Assert.AreEqual("EUR", parsedResponse.CardIssuer.Region);
            Assert.AreEqual("99d787a351cd441c6453733ed65c0c52db2885cb", parsedResponse.Sha1Hash);

            string secretKey = "test1234";

            string correctExpectedHash = parsedResponse.CalculateExpectedSha1Hash(secretKey);
            Assert.AreEqual(correctExpectedHash, parsedResponse.Sha1Hash, "SHA signature");
            Assert.IsTrue(parsedResponse.IsSha1HashCorrect(secretKey));
            Assert.IsFalse(parsedResponse.IsSha1HashCorrect("wrongkey"), "expected mismatch Sha1Hash if the wrong key is used");
        }

        /// <summary>
        /// parse sample response from RealAuth "query"
        /// </summary>
        [TestMethod]
        public void BadQuery()
        {
            string rawRealexResponse =
                "<response timestamp=\"20160701151619\">"
            + "  <result>505</result>"
            + "  <message>You are not allowed to access this service from there! (212.36.61.2)</message>"
            + "  <orderid>216e5f63-6f70-4c56-bea5-43c52441a567</orderid>"
            + "</response>";

            QueryPaymentResponse parsedResponse = RealAuthResponseParser<QueryPaymentResponse>.DeserializeFromString(rawRealexResponse, System.Net.HttpStatusCode.OK, null);
            Assert.IsNotNull(parsedResponse);

            Assert.AreEqual("20160701151619", parsedResponse.Timestamp);
            Assert.AreEqual("505", parsedResponse.ResultString);
            Assert.AreEqual(505, parsedResponse.Result);
            Assert.AreEqual("You are not allowed to access this service from there! (212.36.61.2)", parsedResponse.Message);
            Assert.AreEqual("216e5f63-6f70-4c56-bea5-43c52441a567", parsedResponse.OrderId);
            Assert.IsTrue(parsedResponse.IsSha1HashCorrect("na"));

            Assert.IsFalse(parsedResponse.IsSuccess);
        }

        /*
         * Good Request, but for failed payment
         * HTTP response 200
         * 
<response timestamp="20160704165046">
  <merchantid>energykidzltd</merchantid>
  <account>keephatch</account>
  <orderid>31b9ce58-f464-443c-ae5b-34e0ff6fd708</orderid>
  <authcode></authcode>
  <result>101</result>
  <cvnresult>M</cvnresult>
  <avspostcoderesponse>U</avspostcoderesponse>
  <avsaddressresponse>U</avsaddressresponse>
  <batchid>-1</batchid>
  <message>DECLINED</message>
  <pasref>14676439676545715</pasref>
  <timetaken>0</timetaken>
  <authtimetaken>0</authtimetaken>
  <cardnumber>542011XXXXXX7280</cardnumber>
  <cardissuer>
    <bank>HSBC BANK PLC</bank>
    <country>UNITED KINGDOM</country>
    <countrycode>GB</countrycode>
    <region></region>
  </cardissuer>
  <tss>
    <result></result>
  </tss>
  <threedsecure>
    <cavv></cavv>
    <eci></eci>
    <xid></xid>
  </threedsecure>
  <sha1hash>84404acc2abd3beda7d678461cf8c4105b68db99</sha1hash>
</response>         */

        /*
         * TransactionId not found
<response timestamp="20160704170715">
  <result>508</result>
  <message>Original transaction not found.</message>
  <orderid>216e5f63-6f70-4c56-bea5-43c52441a568</orderid>
</response>         */
    }
}
 