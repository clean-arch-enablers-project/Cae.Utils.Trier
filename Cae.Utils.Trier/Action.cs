using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cae.Utils.MappedExceptions.Specifics;
using Cae.Utils.MappedExceptions;
using Cae.Utils.Trier.AutoRetry;

namespace Cae.Utils.Trier
{
    public abstract class Action<I, O>
    {
        public Task<O> Execute(I input, Dictionary<Type, AutoRetryPolicy> retryPolicies)
        {
            try
            {
                return ExecuteInternalAction(input);
            }
            catch (NoRetriesLeftException)
            {
                throw;
            }
            catch (Exception problem)
            {
                var retryPolicyByException = GetRetryPolicyBy(problem, retryPolicies);
                if (retryPolicyByException != null)
                {
                    var delayToWait = retryPolicyByException
                        .AutoRetryTracker
                        .TrackNewAttemptOn(problem);
                    return RunRetryOn(input, retryPolicies, delayToWait);
                }
                else
                {
                    throw;
                }
            }
        }

        private Task<O> RunRetryOn(I input, Dictionary<Type, AutoRetryPolicy> retryPolicies, int delayToWait)
        {
            try
            {
                return AsyncDelayScheduler.ScheduleWithDelay(
                    () => Execute(input, retryPolicies),  
                    delayToWait
                );
            }
            catch (AggregateException completionException)
            {
                var innerException = completionException.InnerException;
                if (innerException is MappedException exception)
                {
                    throw exception;
                }

                throw new InternalMappedException(
                    "Something went unexpectedly wrong while trying to perform retry at the Action level",
                "More details: " + innerException
                );

            }
        }

        private AutoRetryPolicy? GetRetryPolicyBy(Exception exception, Dictionary<Type, AutoRetryPolicy> retryPolicies)
        {
            return retryPolicies.ContainsKey(exception.GetType()) ? retryPolicies[exception.GetType()] : null;
        }

        protected abstract Task<O> ExecuteInternalAction(I input);
    }
}
