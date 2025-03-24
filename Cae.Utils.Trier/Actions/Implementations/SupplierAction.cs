using Void = Cae.Utils.Trier.Types.Void;

namespace Cae.Utils.Trier.Actions.Implementations;

public class SupplierAction<TO> : Action<Void?, TO>
{
    private readonly Func<Task<TO>>? _supplierAsync;
    private readonly Func<TO>? _supplier;

    public SupplierAction(Func<Task<TO>> supplierAsync) => _supplierAsync = supplierAsync;
    public SupplierAction(Func<TO> supplier) => _supplier = supplier;
    
    protected override TO ExecuteInternalAction(Void? input)
    {
        if (_supplier == null) throw new Exception();

        return _supplier();
    }

    protected override Task<TO> ExecuteInternalActionAsync(Void? input)
    {
        if (_supplierAsync == null) throw new Exception();

        return _supplierAsync();
    }
}