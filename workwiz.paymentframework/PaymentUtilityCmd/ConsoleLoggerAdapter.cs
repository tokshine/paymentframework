using System;
using Workwiz.Common.Logging;

namespace PaymentUtilityCmd
{
    internal class ConsoleLoggerAdapter : IWorkwizLoggerFactory
    {
        public void CreateEntry(Type contextForLogger, LogLevel level, string logMessage, Exception ex)
        {
            Console.WriteLine($"{level} {contextForLogger} {logMessage}");
            if (null != ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}