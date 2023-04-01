using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterMovement), typeof(Attacker), typeof(CharacterAnimator))]
[RequireComponent(typeof(CharacterAudioController))]
public class CharacterController : MonoBehaviour
{
    private InputProvider _inputProvider;

    private Attacker Attacker { get; set; }
    public Rigidbody Rigidbody { get; private set; }
    public CharacterAudioController AudioController { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public CharacterAnimator Animator { get; private set; }
    public CharacterTargetLock TargetLock { get; private set; }
    
    private void Awake()
    {
        //TODO прокинуть через Zenject
        _inputProvider = GameManager.Instance.InputProvider;
        Movement = GetComponent<CharacterMovement>();
        Attacker = GetComponent<Attacker>();
        TargetLock = GetComponent<CharacterTargetLock>();
        Animator = GetComponent<CharacterAnimator>();
        Rigidbody = GetComponent<Rigidbody>();
        AudioController = GetComponent<CharacterAudioController>();
    }

    private void OnEnable()
    {
        _inputProvider.Attacked += OnAttacked;
        _inputProvider.Fired += OnFired;
    }

    private void OnAttacked()
    {
        Attacker.StartAttack();
    }

    private void OnFired()
    {
        Attacker.Fire();
    }

    private void OnDisable()
    {
        _inputProvider.Attacked -= OnAttacked;
        _inputProvider.Fired -= OnFired;
    }
}