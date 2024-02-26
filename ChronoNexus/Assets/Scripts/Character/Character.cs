using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Rigidbody), typeof(Outfitter))]
[RequireComponent(typeof(CharacterMovement), typeof(Attacker), typeof(CharacterAnimator))]
[RequireComponent(typeof(CharacterAudioController))]
[Fix]
public class Character : MonoBehaviour, IDamagable, ITargetable
{
    //Я не знаю как лучше сделать, поэтому просто через префаб пока прокидываем точку.
    [SerializeField] private Transform _aimTarget;

    [SerializeField] private Slider _hpBar;

    [SerializeField] private LevelController _levelController;

    private IOutfitter _outfitter;
    private IHealth _health;

    public Transform AimTarget => _aimTarget;
    public Rigidbody Rigidbody { get; private set; }
    public CharacterAudioController AudioController { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public CharacterAnimator Animator { get; private set; }
    public СharacterTargetingSystem CharacterTargetingSystem { get; private set; }

    private Attacker Attacker { get; set; }

    private bool _isValid = true;
    public Transform Transform => transform;

    private void OnEnable()
    {
        _health.Died += Die;
    }

    private void OnDisable()
    {
        _health.Died -= Die;
    }

    private void Awake()
    {
        //TODO прокинуть через Zenject
        _outfitter = GetComponent<Outfitter>();
        _health = GetComponent<Health>();
        Movement = GetComponent<CharacterMovement>();
        Attacker = GetComponent<Attacker>();
        CharacterTargetingSystem = GetComponent<СharacterTargetingSystem>();
        Animator = GetComponent<CharacterAnimator>();
        Rigidbody = GetComponent<Rigidbody>();
        AudioController = GetComponent<CharacterAudioController>();
    }
    private void Start()
    {
        _hpBar.maxValue = _health.Value;
        _hpBar.value = _health.Value;
    }

    public void TakeDamage(float damage)
    {
        _health.Decrease(damage);
    }

    public void Die()
    {
        _isValid = false;
        OnTargetInvalid?.Invoke();
        Death().Forget();
    }

    private async UniTaskVoid Death()
    {
        Destroy(gameObject);
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        _levelController.Restart();
        await UniTask.Yield();
    }

    public void SetSelfTarget(bool isActive)
    {
        throw new NotImplementedException();
    }

    public Transform GetTransform()
    {
        return _aimTarget;
    }

    public bool GetTargetValid()
    {
        return _isValid;
    }

    public event Action OnTargetInvalid;
}