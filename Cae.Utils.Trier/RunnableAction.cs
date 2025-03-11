using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cae.Utils.Trier
{
    public class RunnableAction : Action<Void, Void>
    {
        private readonly Action _runnable;

        public RunnableAction(Action runnable)
        {
            _runnable = runnable ?? throw new ArgumentNullException(nameof(runnable));
        }

        protected override Task<Void> ExecuteInternalAction(Void input)
        {
            _runnable();
            return null;
        }
    }
}
