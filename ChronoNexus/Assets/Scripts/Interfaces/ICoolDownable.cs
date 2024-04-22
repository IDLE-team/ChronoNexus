using System;

public interface ICoolDownable
{
    public event Action<float> OnCoolDown;
}