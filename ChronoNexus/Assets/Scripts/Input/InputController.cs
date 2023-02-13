using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public event Action OnAttack;

    public event Action OnDash;

    public event Action OnSkill;

    public event Action OnTurnOffTargetLock;

    public event Action OnSetTarget;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetTarget();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            TurnOffTargetLock();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    public void Attack()
    {
        OnAttack?.Invoke();
    }

    public void Dash()
    {
        OnDash?.Invoke();
    }

    public void Skill()
    {
        OnSkill?.Invoke();
    }

    public void SetTarget()
    {
        OnSetTarget?.Invoke();
    }

    public void TurnOffTargetLock()
    {
        OnTurnOffTargetLock?.Invoke();
    }

    public void Restart()
    {
        GameManager.instance.RestartGame();
    }
}