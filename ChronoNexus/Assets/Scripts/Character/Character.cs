using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Outfitter))]
[RequireComponent(typeof(CharacterMovement), typeof(Attacker), typeof(CharacterAnimator))]
[RequireComponent(typeof(CharacterAudioController))]
[Fix]
public class Character : MonoBehaviour
{
    private IOutfitter _outfitter;
    private Attacker Attacker { get; set; }
    public Rigidbody Rigidbody { get; private set; }
    public CharacterAudioController AudioController { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public CharacterAnimator Animator { get; private set; }
    public CharacterTargetLock TargetLock { get; private set; }
    
    private void Awake()
    {
        //TODO прокинуть через Zenject
        _outfitter = GetComponent<Outfitter>();
        Movement = GetComponent<CharacterMovement>();
        Attacker = GetComponent<Attacker>();
        TargetLock = GetComponent<CharacterTargetLock>();
        Animator = GetComponent<CharacterAnimator>();
        Rigidbody = GetComponent<Rigidbody>();
        AudioController = GetComponent<CharacterAudioController>();
    }
}