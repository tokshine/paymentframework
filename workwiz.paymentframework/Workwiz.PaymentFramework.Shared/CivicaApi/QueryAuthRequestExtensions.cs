using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workwiz.PaymentFramework.Shared.CivicaAuthRequest;

namespace Workwiz.PaymentFramework.Shared.CivicaApi
{
    public static class QueryAuthRequestExtensions
    {
        public static RespMessage Query(
            this QueryAuthRequestSrvSoap svcClient,
            Workwiz.PaymentFramework.Shared.CivicaAuthRequest.ReqMessage reqMessage)
        {
            QueryRequest inValue = new Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryRequest()
            {
                ReqMessage = reqMessage
            };
            QueryResponse retVal = svcClient.Query(inValue);
            return retVal.RespMessage;
        }

        public static async Task<RespMessage> QueryAsync(
            this QueryAuthRequestSrvSoap svcClient,
            ReqMessage reqMessage)
        {
            QueryRequest inValue = new Workwiz.PaymentFramework.Shared.CivicaAuthRequest.QueryRequest();
            inValue.ReqMessage = reqMessage;
            QueryResponse retVal = await svcClient.QueryAsync(inValue);
            return retVal.RespMessage;
        }
    }
}
