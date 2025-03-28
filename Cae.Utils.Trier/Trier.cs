using Cae.Utils.MappedExceptions;
using Cae.Utils.Trier.Actions.Implementations;
using Cae.Utils.Trier.Exceptions;

namespace Cae.Utils.Trier;

public class Trier<T,TO>
{
    private readonly Actions.Action<T,TO> _action;
    private readonly T _input;
    private readonly IUnexpectedExceptionHandler _unexpectedExceptionHandler;

#pragma warning disable CS8618, CS8601
    public Trier(Actions.Action<T, TO> action, IUnexpectedExceptionHandler unexpectedExceptionHandler, T ? input = default)
    {
        _action = action;

        if (_action is not (RunnableAction or SupplierAction<TO>)
            && EqualityComparer<T>.Default.Equals(input, default))
        {
            throw new Exception("Input cannot be null for this action type.");
        }

        _input = input;
        _unexpectedExceptionHandler = unexpectedExceptionHandler;
    }

#pragma warning restore

    public static TrierBuilder<T, TO> CreateInstance(Actions.Action<T, TO> action, T? input)
    {
        return new TrierBuilder<T,TO>(action, input);
    }

    public TO Execute()
    {
        try
        {
            return _action.Execute(_input);
        }
        catch (MappedException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw _unexpectedExceptionHandler.Handle(e);
        }
    }

    public Task<TO> ExecuteAsync()
    {
        try
        {
            return _action.ExecuteAsync(_input);
        }
        catch (MappedException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw _unexpectedExceptionHandler.Handle(e);
        }
    }
}