using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cae.Utils.Trier
{
    public class ConsumerAction<I> : Action<I, Void>
    {
        private readonly System.Action<I> _action;

        public ConsumerAction(System.Action<I> action)
        {
            _action = action;
        }

        protected override Task<Void> ExecuteInternalAction(I input)
        {
            _action(input);
            return null;
        }
    }
}
