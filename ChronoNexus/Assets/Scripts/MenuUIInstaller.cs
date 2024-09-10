using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class MenuUIInstaller : MonoInstaller
{
    [SerializeField] private GameObject _menuUi;
    
    public override void InstallBindings()
    {
        //  GameObject ui = Container.InstantiatePrefab(_menuUi);
        //GameObject ui = Container.InstantiatePrefab(_menuUi);
        
        Container.Bind<InventoryItemManager>().FromInstance(_menuUi.GetComponent<UIPrefabData>().InventoryItemManager).AsSingle().NonLazy();
        Container.Bind<WinScreen>().FromInstance(_menuUi.GetComponent<UIPrefabData>().WinScreen).AsSingle().NonLazy();
        Container.Bind<DeathScreen>().FromInstance(_menuUi.GetComponent<UIPrefabData>().DeathScreen).AsSingle().NonLazy();

    }
}
