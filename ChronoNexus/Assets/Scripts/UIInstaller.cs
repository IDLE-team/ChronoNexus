using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class UIInstaller : MonoInstaller
{
    [SerializeField] private GameObject _ui;
    public override void InstallBindings()
    {
        //GameObject ui = Container.InstantiatePrefab(_ui); 
        //Container.Bind<MainButtonController>().FromInstance(ui.GetComponent<UIPrefabData>().MainButtonController).AsSingle();c
        // Container.Bind<InventoryItemManager>().FromInstance(ui.GetComponent<UIPrefabData>().InventoryItemManager).AsSingle();

    }
}
