using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int TakeHit = Animator.StringToHash("TakeHit");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Death()
    {
        _animator.SetBool(Dead, true);
    }
    
    public void TakeDamage()
    {
        _animator.SetTrigger(TakeHit);
    }
}