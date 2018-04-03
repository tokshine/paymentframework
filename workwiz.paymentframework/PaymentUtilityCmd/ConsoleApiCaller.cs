using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workwiz.PaymentFramework.Shared;
using Workwiz.PaymentFramework.Shared.Models;

namespace PaymentUtilityCmd
{
    internal class ConsoleApiCaller
    {
        private readonly IPaymentProviderBackend _apiProvider;
        private readonly PaymentProviderConfiguration _providerConfig;

        internal ConsoleApiCaller(
            IPaymentProviderBackend apiProvider,
            PaymentProviderConfiguration providerConfig)
        {
            this._apiProvider = apiProvider;
            this._providerConfig = providerConfig;
        }

        internal async Task DoCheckAuth(string recordId)
        {
            Console.WriteLine($"Calling apiProvider.CheckAuthorization({recordId})");
            try
            {
                var response = await _apiProvider.CheckAuthorization(_providerConfig, recordId, 123.45m, string.Empty);
                Console.WriteLine($"{response.Description}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught {ex}");
            }
            Console.WriteLine("Done");
        }
    }
}
