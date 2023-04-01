using System;
using UnityEngine;

[Fix]
public class InputService : MonoBehaviour, IInputService
{
    public event Action Attacked;
    public event Action Shot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Fire();
        }
    }

    public void Attack() => Attacked?.Invoke();

    public void Fire() => Shot?.Invoke();
}