using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cae.Utils.Trier.AutoRetry
{
    public class OnExhaustion
    {
        public delegate void HandleFailure(FailureStatus failureStatus);

        public class FailureStatus
        {
            public Exception Exception { get; }
            public int TotalOfRetries { get; }

            public static FailureStatus CreateInstance(Exception exception, int totalOfRetries)
            {
                return new FailureStatus(exception, totalOfRetries);
            }

            private FailureStatus(Exception exception, int totalOfRetries)
            {
                Exception = exception;
                TotalOfRetries = totalOfRetries;
            }
        }
    }
}
