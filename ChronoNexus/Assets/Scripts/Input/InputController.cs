using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public event Action OnAttack;

    public event Action OnFire;

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

    public void Attack()
    {
        OnAttack?.Invoke();
    }

    public void Fire()
    {
        OnFire?.Invoke();
    }

    public void Restart()
    {
        GameManager.instance.RestartGame();
    }
}