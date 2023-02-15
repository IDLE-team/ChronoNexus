using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterMovement), typeof(CharacterAttack), typeof(CharacterAnimation))]
[RequireComponent(typeof(CharacterAudioController))]
public class CharacterController : MonoBehaviour
{
    private InputController _inputController;

    public Rigidbody _rigidbody { get; private set; }
    public Animator _animator { get; private set; }
    public CharacterAudioController _audioController { get; private set; }
    public CharacterMovement _characterMovement { get; private set; }
    public CharacterAttack _characterAttack { get; private set; }
    public CharacterAnimation _characterAnimation { get; private set; }
    public CharacterTargetLock _characterTargetLock { get; private set; }

    public bool _hasAnimator { get; private set; }

    private void Awake()
    {
        _inputController = GameManager.instance.inputController;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _characterMovement = GetComponent<CharacterMovement>();
        _characterAttack = GetComponent<CharacterAttack>();
        _characterTargetLock = GetComponent<CharacterTargetLock>();
        _characterAnimation = GetComponent<CharacterAnimation>();
        _audioController = GetComponent<CharacterAudioController>();
    }

    private void Attack()
    {
        _characterAttack.StartAttack();
    }

    private void Fire()
    {
        _characterAttack.Fire();
    }

    private void OnEnable()
    {
        _inputController.OnAttack += Attack;
        _inputController.OnFire += Fire;
    }

    private void OnDisable()
    {
        _inputController.OnAttack -= Attack;
        _inputController.OnFire -= Fire;
    }
}