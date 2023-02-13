
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerMovement), typeof(PlayerAttack), typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerAudioController))]
public class PlayerController : MonoBehaviour
{
    private InputController _inputController;

    protected Rigidbody _rigidbody;
    protected Animator _animator;
    protected PlayerAudioController _audioController;
    protected PlayerMovement _playerMovement;
    protected PlayerAttack _playerAttack;
    protected PlayerAnimation _playerAnimation;

    [SerializeField] protected bool _hasAnimator;

    private void Awake()
    {
        _inputController = GameManager.instance.inputController;
    }

    private void Start()
    {
        _audioController = GetComponent<PlayerAudioController>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _hasAnimator = TryGetComponent(out _animator);
    }

    private void Attack()
    {
        _playerAttack.StartAttack();
    }

    private void TurnOffTargetLock()
    {
        _playerMovement.TurnOffTargetLock();
    }

    private void ChangeTarget()
    {
        _playerMovement.LookAtTarget();
    }

    private void OnEnable()
    {
        _inputController.OnAttack += Attack;
        _inputController.OnTurnOffTargetLock += TurnOffTargetLock;
        _inputController.OnSetTarget += ChangeTarget;
    }

   private void OnDisable()
    {
        _inputController.OnAttack -= Attack;
        _inputController.OnTurnOffTargetLock -= TurnOffTargetLock;
        _inputController.OnSetTarget -= ChangeTarget;
    }
}