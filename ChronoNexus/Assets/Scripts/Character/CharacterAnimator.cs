using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterAnimator : MonoBehaviour
{
    //TODO подумать насчёт суффикса "Hash"
    private Animator _animator;
    
    private int _currentFinisherID;
    public int CurrentFinisherID => _currentFinisherID;
    
    private static readonly int StrafeHash = Animator.StringToHash("Strafe");
    private static readonly int StrafeXHash = Animator.StringToHash("StrafeX");
    private static readonly int StrafeZHash = Animator.StringToHash("StrafeZ");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");
    private static readonly int JumpHashHash = Animator.StringToHash("Jump");
    private static readonly int FreeFallHash = Animator.StringToHash("FreeFall");
    private static readonly int MotionSpeedHash = Animator.StringToHash("MotionSpeed");
    private static readonly int TurnHash = Animator.StringToHash("Turn");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private static readonly int SitHash = Animator.StringToHash("Sit");
    private static readonly int FinisherHash = Animator.StringToHash("Finisher");


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StrafeX(float value)
    {
        _animator.SetFloat(StrafeXHash, value);
    }

    public void StrafeZ(float value)
    {
        _animator.SetFloat(StrafeZHash, value);
    }

    public void MotionSpeed(float value)
    {
        _animator.SetFloat(MotionSpeedHash, value);
    }

    public void Attack()
    {
        _animator.SetTrigger(AttackHash);
    }

    public void Finisher()
    {
        _animator.SetTrigger(FinisherHash);
        _currentFinisherID = Random.Range(0, 7);
        _animator.SetInteger("FinisherID",_currentFinisherID );
    }

    public void Fire(int hash)
    {
        _animator.SetTrigger(hash);
    }
    public void Sit(bool value)
    {
        _animator.SetBool(SitHash, value);
    }
}