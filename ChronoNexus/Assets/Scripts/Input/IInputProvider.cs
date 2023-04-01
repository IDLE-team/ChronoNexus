using System;

public interface IInputProvider
{
    public event Action Attacked;
    public event Action Fired;
}