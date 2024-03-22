using UnityEngine;
using Zenject;
public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _spawnPoint;
    public override void InstallBindings()
    {
        GameObject player = Container.InstantiatePrefab(_player);
        player.transform.position = _spawnPoint.position;
        
        Container.Bind<CharacterMovement>().FromInstance(player.GetComponent<PlayerPrefabData>().Character.Movement).AsSingle();
        Container.Bind<CharacterAnimator>().FromInstance(player.GetComponent<PlayerPrefabData>().Character.Animator).AsSingle();

    }
}
