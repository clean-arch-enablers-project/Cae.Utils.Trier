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
    public class Trier<I, O>
    {
        private readonly Action<I, O> _action;
        private readonly I _input;
        private readonly IUnexpectedExceptionHandler _unexpectedExceptionHandler;
        private readonly Dictionary<Type, AutoRetryPolicy> _retryPolicies;
        private readonly OnExhaustion? _onExhaustion;

        private Trier(
            Action<I, O> action,
            I input,
            IUnexpectedExceptionHandler unexpectedExceptionHandler,
            Dictionary<Type, AutoRetryPolicy> retryPolicies,
            OnExhaustion? onExhaustion = null)
        {
            _action = action;
            _input = input;
            _unexpectedExceptionHandler = unexpectedExceptionHandler;
            _retryPolicies = retryPolicies;
            _onExhaustion = onExhaustion;
        }

        public static TrierBuilder<I, O> Of(Func<I, O> function, I input)
        {
            return new TrierBuilder<I, O>(ActionFactory.CreateInstance(function), input);
        }

        public static TrierBuilder<I, Void> Of(Action<I> consumer, I input)
        {
            return new TrierBuilder<I, Void>(ActionFactory.CreateInstance(consumer), input);
        }

        public static TrierBuilder<Void, O> Of(Func<O> supplier)
        {
            return new TrierBuilder<Void, O>(ActionFactory.CreateInstance(supplier), default!);
        }

        public static TrierBuilder<Void, Void> Of(Action runnable)
        {
            return new TrierBuilder<Void, Void>(ActionFactory.CreateInstance(runnable), default!);
        }

        public async Task<O> Execute()
        {
            try
            {
                return await _action.Execute(_input, _retryPolicies);
            }
            catch (NoRetriesLeftException ex)
            {
                //_onExhaustion?.Handle(ex.FailureStatus)
                //    ?? throw new InternalMappedException(
                //        "Exhausted all retries but no circuit-break setting was provided",
                //        "If you are dealing with retries, make sure you set a circuit-break option by calling the 'onFailure' method while building the Trier object");

                if (_onExhaustion != null)
                {
                    _onExhaustion.Handle(OnExhaustion.FailureStatus.CreateInstance(ex, 0));
                    throw;
                }

                throw new InternalMappedException(
                        "Exhausted all retries but no circuit-break setting was provided",
                        "If you are dealing with retries, make sure you set a circuit-break option by calling the 'onFailure' method while building the Trier object");
            }
            catch (MappedException)
            {
                throw;
            }
            catch (Exception unexpectedException)
            {
                throw _unexpectedExceptionHandler.Handle(unexpectedException);
            }
        }

        public class TrierBuilder<I, O>
        {
            private readonly Action<I, O> _action;
            private readonly I _input;
            private readonly Dictionary<Type, AutoRetryPolicy> _retryPolicies = new();
            private OnExhaustion? _onExhaustion;

            public TrierBuilder(Action<I, O> action, I input)
            {
                _action = action;
                _input = input;
            }

            public TrierBuilder<I, O> AutoRetryOn<E>(int maxAmountOfRetries, int baseTimeInSeconds) where E : Exception
            {
                if (typeof(E) != typeof(NoRetriesLeftException))
                    _retryPolicies[typeof(E)] = AutoRetryPolicy.CreatePolicy(maxAmountOfRetries, baseTimeInSeconds);
                return this;
            }

            public TrierBuilder<I, O> OnExhaustion(OnExhaustion onExhaustion)
            {
                _onExhaustion = onExhaustion;
                return this;
            }

            public Trier<I, O> SetUnexpectedExceptionHandler(IUnexpectedExceptionHandler handler)
            {
                return new Trier<I, O>(_action, _input, handler, _retryPolicies, _onExhaustion);
            }
        }
    }
}
