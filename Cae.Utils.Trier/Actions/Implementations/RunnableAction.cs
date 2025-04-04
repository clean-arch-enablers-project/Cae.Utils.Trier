using VoidReturn = Cae.Utils.Trier.Types.VoidReturn;

namespace Cae.Utils.Trier.Actions.Implementations;

public class RunnableAction : Action<VoidReturn?, VoidReturn?>
{
    private readonly Action? _action;
    private readonly Func<Task>? _actionAsync;

    public RunnableAction(Action action) => _action = action;
    public RunnableAction(Func<Task> actionAsync) => _actionAsync = actionAsync;

    protected override VoidReturn? ExecuteInternalAction(VoidReturn? input = null)
    {
        if (_action == null) throw new Exception();
        
        _action.Invoke();
        return null;
    }

    protected override async Task<VoidReturn?> ExecuteInternalActionAsync(VoidReturn? input = null)
    {
        if (_actionAsync == null) throw new Exception();
        
        await _actionAsync();
        return null;
    }
}