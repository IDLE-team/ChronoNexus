using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private void Update()
    {
        transform.position = _player.transform.position;
    }
}
