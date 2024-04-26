using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private void Update()
    {
        if(_player) 
            transform.position = _player.transform.position;
    }
}
