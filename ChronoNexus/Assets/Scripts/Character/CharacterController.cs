using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterMovement), typeof(CharacterAttack), typeof(CharacterAnimation))]
[RequireComponent(typeof(CharacterAudioController))]
[RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviour
{
    private InputController _inputController;

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterAudioController AudioController { get; private set; }
    public CharacterMovement CharacterMovement { get; private set; }
    public CharacterAttack CharacterAttack { get; private set; }
    public CharacterAnimation CharacterAnimation { get; private set; }
    public CharacterTargetLock CharacterTargetLock { get; private set; }

    public bool _hasAnimator { get; private set; }

    private void Awake()
    {
        _inputController = GameManager.instance.inputController;

        CharacterMovement = GetComponent<CharacterMovement>();
        CharacterAttack = GetComponent<CharacterAttack>();
        CharacterTargetLock = GetComponent<CharacterTargetLock>();
        CharacterAnimation = GetComponent<CharacterAnimation>();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        AudioController = GetComponent<CharacterAudioController>();
    }

    private void Attack()
    {
        CharacterAttack.StartAttack();
    }

    private void Fire()
    {
        CharacterAttack.Fire();
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