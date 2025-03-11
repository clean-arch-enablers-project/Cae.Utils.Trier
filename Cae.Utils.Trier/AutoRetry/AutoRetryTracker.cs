using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cae.Utils.Trier.AutoRetry
{
    public class AutoRetryTracker
    {
        public static AutoRetryTracker CreateTracker(AutoRetryPolicy autoRetryPolicy)
        {
            return new AutoRetryTracker
            {
                NumberOfRetriesTriggered = 0,
                MaxAmountOfRetriesAllowed = autoRetryPolicy.MaxAmountOfRetries,
                BaseTimeInSeconds = autoRetryPolicy.BaseTimeInSeconds
            };
        }

        private int NumberOfRetriesTriggered { get; set; }
        private int MaxAmountOfRetriesAllowed { get; set; }
        private int BaseTimeInSeconds { get; set; }

        public int TrackNewAttemptOn(Exception ex)
        {
            if (NumberOfRetriesTriggered < MaxAmountOfRetriesAllowed)
            {
                int delayToRetry = (int)(BaseTimeInSeconds * Math.Pow(2, NumberOfRetriesTriggered));

                NumberOfRetriesTriggered += 1;

                return delayToRetry;
            }

            throw new NoRetriesLeftException(
                "Couldn't proceed with more retries",
                OnExhaustion.FailureStatus.CreateInstance(ex, NumberOfRetriesTriggered));
        }
    }
}
