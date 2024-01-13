using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;
    private float _lastSpeed;
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int TakeHit = Animator.StringToHash("TakeHit");
    private static readonly int Moving = Animator.StringToHash("isMove");

    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartMoveAnimation()
    {
        _animator.SetBool(Moving, true);
    }

    public void EndMoveAnimation()
    {
        _animator.SetBool(Moving, false);
    }

    public void PlayDeathAnimation()
    {
        _animator.SetBool(Dead, true);
    }

    public void PlayTakeDamageAnimation()
    {
        _animator.SetTrigger(TakeHit);
    }

    public void StopAnimation()
    {
        _lastSpeed = _animator.speed;
        _animator.speed = 0;
        Debug.Log("������ ���� ��������� ��������");
    }
    public void SlowAnimation()
    {
        _lastSpeed = _animator.speed;
        _animator.speed = 0.3f;
        Debug.Log("������ ���� ��������� ��������");
    }
    public void ContinueAnimation()
    {
        _animator.speed = 1;

    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(Attack);
    }

    public void PlayShootAnimation()
    {
        _animator.SetTrigger(Shoot);
    }
}