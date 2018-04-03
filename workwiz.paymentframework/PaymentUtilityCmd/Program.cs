using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Workwiz.PaymentFramework.Shared;
using Workwiz.PaymentFramework.Shared.Models;

namespace PaymentUtilityCmd
{
    class Program
    {
        public class CmdOptions
        {
            [Option('c', "command", DefaultValue = "CheckAuth", HelpText = "Command to run")]
            public string CommandName { get; set; }

            [Option('p', "provider", DefaultValue = ProviderType.Realex, HelpText = "Payment Provider Type")]
            public ProviderType Provider { get; set; }

            [Option('m', "merchantid", DefaultValue = "workwiz", HelpText = "Merchant ID")]
            public string MerchantId { get; set; }

            [Option('s', "secretkey", DefaultValue = "secretKey", HelpText = "Secret Key")]
            public string SecretKey { get; set; }

            [Option("subaccount", DefaultValue = "internet", HelpText = "Sub-Account")]
            public string SubAccount { get; set; }

            [Option('i', "recordid", DefaultValue = "secretKey", HelpText = "Secret Key")]
            public string RecordId { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        static void Main(string[] args)
        {
            try
            {
                MainAsync(args).Wait();
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine($"Aggregate Exception: {ex}");
                }
            }
        }

        static async Task MainAsync(string [] args)
        {
            CmdOptions options = new CmdOptions();
            bool didParseOptions = CommandLine.Parser.Default.ParseArguments(args, options);

            if (!didParseOptions)
            {
                Console.WriteLine("Error: failed to parse command line options");
                return;
            }

            IPaymentProviderBackend apiProvider = PaymentProviderBackendFactory.GetProvider(options.Provider, new ConsoleLoggerAdapter());
            PaymentProviderConfiguration providerConfig = new PaymentProviderConfiguration()
            {
                AccountIdentifer = options.MerchantId,
                SharedSecret = options.SecretKey
            };
            ConsoleApiCaller apiCaller = new ConsoleApiCaller(apiProvider, providerConfig);

            switch (options.CommandName)
            {
                case "checkAuth":
                    await apiCaller.DoCheckAuth(options.RecordId /* , options.SubAccount */);
                    break;
                default:
                    Console.WriteLine($"Error: command \"{options.CommandName}\" not supporter");
                    break;
            }
        }
    }
}
