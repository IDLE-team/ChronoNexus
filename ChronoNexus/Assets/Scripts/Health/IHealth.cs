using System;

public interface IHealth
{
    public float Value { get; }
    public event Action Died;
    public event Action<float> Changed;

    public void Decrease(float value);
}