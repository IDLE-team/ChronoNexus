using UnityEngine;
using Zenject;
public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform _spawnPoint;
    private Health health;
    public override void InstallBindings()
    {
       // GameObject player = Container.InstantiatePrefab(_player);
       // player.transform.position = _spawnPoint.position;
        health = player.GetComponent<PlayerPrefabData>().Character.Health;
        Container.Bind<CharacterMovement>().FromInstance(player.GetComponent<PlayerPrefabData>().Character.Movement).AsSingle();
        Container.Bind<CharacterAnimator>().FromInstance(player.GetComponent<PlayerPrefabData>().Character.Animator).AsSingle();
        Container.Bind<Character>().FromInstance(player.GetComponent<PlayerPrefabData>().Character).AsSingle();
        var eventsHolder = player.GetComponent<PlayerPrefabData>().Character.CharacterEventsHolder;
        //Debug.Log(eventsHolder);
        Container.Bind<CharacterEventsHolder>().FromInstance(player.GetComponent<PlayerPrefabData>().Character.CharacterEventsHolder).AsSingle();
        Container.Bind<Health>().FromInstance(player.GetComponent<PlayerPrefabData>().Character.Health).AsSingle().Lazy();


    }
}
