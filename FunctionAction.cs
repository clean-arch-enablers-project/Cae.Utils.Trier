using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cae.Utils.Trier
{
    public class FunctionAction<I, O> : Action<I, O>
    {
        private readonly Func<I, O> _function;

        public FunctionAction(Func<I, O> function)
        {
            _function = function;
        }

        protected override Task<O> ExecuteInternalAction(I input)
        {
            return Task.FromResult(_function(input));
        }
    }
}
