using System;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class ServiceInstaller : MonoInstaller
{
    [SerializeField] private TimerView _timerView;
    [SerializeField] private GameObject _weaponFactory;
    [SerializeField] private SettinsPrefabData _settinsPrefabData;
    public override void InstallBindings()
    {
        BindInput();
        BindPause();
        BindTimer();
        BindWeaponFactory();
        BindSettings();
    }
    private void BindInput()
    {
        PlayerInputActions input = new PlayerInputActions();

        input.Enable();   
        Container.Bind<PlayerInputActions>().FromInstance(input).AsSingle();

    }

    private void BindWeaponFactory()
    {
       var weaponFactory = Container.InstantiatePrefab(_weaponFactory);
       Container.Bind<WeaponFactory>().FromInstance(weaponFactory.GetComponent<WeaponFactory>()).AsSingle();
    }

    private void BindSettings()
    {
        Container.Bind<Volume>().FromInstance(_settinsPrefabData.PostProcessVolume).AsSingle();
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
