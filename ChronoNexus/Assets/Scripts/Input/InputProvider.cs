using System;
using UnityEngine;

public class InputProvider : MonoBehaviour, IInputProvider
{
    public event Action Attacked;
    public event Action Fired;

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

    public void Fire() => Fired?.Invoke();
}