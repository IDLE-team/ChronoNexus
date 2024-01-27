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
        input.Enable();   
        Container.Bind<PlayerInputActions>().FromInstance(input).AsSingle();
    }
}
