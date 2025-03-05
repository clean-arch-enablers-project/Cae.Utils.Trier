using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cae.Utils.Trier.AutoRetry
{
    public class AutoRetryPolicy
    {
        public static AutoRetryPolicy CreatePolicy(int maxAmountOfRetries, int baseTimeInSeconds)
        {
            return new AutoRetryPolicy(maxAmountOfRetries, baseTimeInSeconds);
        }

        private AutoRetryPolicy(int maxAmountOfRetries, int baseTimeInSeconds)
        {
            BaseTimeInSeconds = baseTimeInSeconds;
            MaxAmountOfRetries = maxAmountOfRetries;
            AutoRetryTracker = AutoRetryTracker.CreateTracker(this);
        }

        public int BaseTimeInSeconds { get; init; }
        public int MaxAmountOfRetries { get; init; }
        public AutoRetryTracker AutoRetryTracker { get; init; }
    }
}
