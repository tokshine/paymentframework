using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Workwiz.PaymentFramework.Shared.RealexApi.RealVault;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealAuth
{
    public class RealAuthResponseParser<TResponse> where TResponse : RealAuthResponse, new() 
    {
        private static readonly XmlSerializer _responseSerializer = new XmlSerializer(typeof(TResponse));

        public static TResponse DeserializeFromString(string realexResponse, HttpStatusCode httpResponseCode, RealAuthRequest originalRequest)
        {
            TResponse parsedResponse;
            try
            {
                using (System.IO.StringReader sr = new System.IO.StringReader(realexResponse))
                {
                    parsedResponse = (_responseSerializer.Deserialize(sr) as TResponse) ?? new TResponse()
                    {
                        ResultString = "-1",
                        Message = $"Failed to deserialze as {typeof(TResponse).FullName}"
                    };
                }
            }
            catch (Exception ex)
            {
                parsedResponse = new TResponse()
                {
                    ResultString = "-1",
                    Message = ex.ToString(),
                };
            }
            parsedResponse.HttpStatus = httpResponseCode;
            parsedResponse.OriginalRequest = originalRequest;
            parsedResponse.RawResponseString = realexResponse;
            return parsedResponse;
        }
    }
}
