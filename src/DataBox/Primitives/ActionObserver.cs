using System;

namespace DataBox.Primitives;

internal class ActionObserver<T> :  IObserver<T>
{
    private readonly Action<T> _action;

    public ActionObserver(Action<T> action)
    {
        _action = action;
    }
    
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(T value)
    {
        _action(value);
    }
}
