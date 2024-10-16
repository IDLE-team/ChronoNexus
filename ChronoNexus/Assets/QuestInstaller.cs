using Zenject;
using UnityEngine;

public class QuestInstaller : MonoInstaller
{
    [SerializeField] private QuestUISetter _questUI;
    
    public override void InstallBindings()
    {
        Container.Bind<QuestUISetter>().FromInstance(_questUI);
    }
}
