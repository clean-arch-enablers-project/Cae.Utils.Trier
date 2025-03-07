using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cae.Utils.Trier
{

    public static class ActionFactory
    {
        public static Action<I, O> CreateInstance<I, O>(Func<I, O> function)
        {
            return new FunctionAction<I, O>(function);
        }

        public static Action<I, Void> CreateInstance<I>(Action<I> consumer)
        {
            return new ConsumerAction<I>(consumer);
        }

        public static Action<Void, O> CreateInstance<O>(Func<O> supplier)
        {
            return new SupplierAction<O>(supplier);
        }

        public static Action<Void, Void> CreateInstance(Action runnable)
        {
            return new RunnableAction(runnable);
        }
    }

    /// <summary>
    /// Just a Dummy class because C# dont have Void as a Type
    /// </summary>
    public class Void
    {
        public static readonly Void Instance = new();
        private Void() { }
    }
}

