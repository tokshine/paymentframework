using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Workwiz.Common.Logging;

namespace Workwiz.PaymentFramework.TestWeb
{
    public class TraceWriterLogAdapter : IWorkwizLoggerFactory
    {
        public void CreateEntry(Type contextForLogger, LogLevel level, string logMessage, Exception ex)
        {
            System.Diagnostics.Trace.WriteLine($"{level} {contextForLogger} {logMessage}", "WorkwizLogging");
            if (null != ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString(), "WorkwizLogging");
            }
        }
    }
}