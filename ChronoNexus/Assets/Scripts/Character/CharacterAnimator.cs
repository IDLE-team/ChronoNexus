using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    //TODO подумать насчёт суффикса "Hash"
    private Animator _animator;

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

    public void Fire()
    {
        _animator.SetTrigger(ShootHash);
    }
    public void Sit(bool value)
    {
        _animator.SetBool(SitHash, value);
    }
}