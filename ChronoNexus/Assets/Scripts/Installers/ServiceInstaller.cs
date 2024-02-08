using System;
using UnityEngine;
using Zenject;

public class ServiceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindInput();
    }
    private void BindInput()
    {
        PlayerInputActions input = new PlayerInputActions();
        CharacterInput input2 = new CharacterInput();

        input.Enable();   
        Container.Bind<PlayerInputActions>().FromInstance(input).AsSingle();
        Container.Bind<CharacterInput>().FromInstance(input2).AsSingle();

    }
}
