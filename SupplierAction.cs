using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cae.Utils.Trier
{
    public class SupplierAction<O> : Action<Void, O>
    {
        private readonly Func<O> _supplier;

        public SupplierAction(Func<O> supplier)
        {
            _supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
        }

        protected override Task<O> ExecuteInternalAction(Void input)
        {
            return Task.FromResult(_supplier());
        }
    }
}
