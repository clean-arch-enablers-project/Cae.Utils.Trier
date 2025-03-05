using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cae.Utils.MappedExceptions.Specifics;

namespace Cae.Utils.Trier.AutoRetry
{
    public class AsyncDelayScheduler
    {
        public static readonly AsyncDelayScheduler Instance = new();

        private AsyncDelayScheduler() { }

        public static async Task<O> ScheduleWithDelay<O>(Func<Task<O>> supplier, int totalDelayInSeconds)
        {
            try
            {
                await Task.Delay(totalDelayInSeconds * 1000);
                return await supplier();
            }
            catch (Exception ex)
            {
                throw new InternalMappedException(
                    "An error occurred during the delay or execution.",
                    "More details: " + ex
                );
            }
        }
    }
}
