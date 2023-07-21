using System;

public interface IInputService
{
    public event Action Attacked;
    public event Action Shot;
}