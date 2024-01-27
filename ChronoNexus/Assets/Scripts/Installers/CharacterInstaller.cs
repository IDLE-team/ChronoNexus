using UnityEngine;
using Zenject;
public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private CharacterAnimator _characterAnimator;

    public override void InstallBindings()
    {
        Container.Bind<CharacterMovement>().FromInstance(_characterMovement).AsSingle();
        Container.Bind<CharacterAnimator>().FromInstance(_characterAnimator).AsSingle();

    }
}
