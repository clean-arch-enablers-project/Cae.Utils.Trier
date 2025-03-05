using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cae.Utils.MappedExceptions;
using static Cae.Utils.Trier.AutoRetry.OnExhaustion;

namespace Cae.Utils.Trier.AutoRetry
{
    public class NoRetriesLeftException : MappedException
    {
        public OnExhaustion.FailureStatus FailureStatus { get; init; }

        public NoRetriesLeftException(string briefPublicMessage, OnExhaustion.FailureStatus failureStatus) : base(
                    briefPublicMessage,
                    "All retries (" + failureStatus.TotalOfRetries + ") done for " + failureStatus.Exception
            )
        {

            FailureStatus = failureStatus;
        }
    }
}
