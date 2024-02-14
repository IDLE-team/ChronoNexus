using System;
using UnityEngine;
using Zenject;

public class ServiceInstaller : MonoInstaller
{
    [SerializeField] private TimerView _timerView;
    public override void InstallBindings()
    {
        BindInput();
        BindPause();
        BindTimer();
    }
    private void BindInput()
    {
        PlayerInputActions input = new PlayerInputActions();

        input.Enable();   
        Container.Bind<PlayerInputActions>().FromInstance(input).AsSingle();

    }
    private void BindTimer()
    {
        Container.BindInstance(new Timer(this)).AsSingle();
        Container.Bind<TimerView>().FromInstance(_timerView).AsSingle();
    }

    private void BindPause()
    {
        Container.BindInterfacesAndSelfTo<PauseHandler>().AsSingle();
    }
}
