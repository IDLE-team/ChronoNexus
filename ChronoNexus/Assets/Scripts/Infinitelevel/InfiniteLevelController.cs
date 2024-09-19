using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using NavMeshSurface = Unity.AI.Navigation.NavMeshSurface;
using Random = UnityEngine.Random;

public class InfiniteLevelController : MonoBehaviour
{ 
    
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject[] _startRoom;
    [SerializeField] private GameObject[] _exitRoom;
    [SerializeField] private GameObject[] _room;
    [SerializeField] private List<GameObject> _entityList;
    [SerializeField] private int _entityInRoomCount = 3;
    [SerializeField] private NavMeshSurface _navMesh;
    
    private TransportRoom _transportRoomTemp1;
    private TransportRoom _transportRoomTemp2;
    private Room _roomTemp;
    private int _entityInRoomCountTemp;
    
    private List<GameObject> _entityListTemp;

    private bool _transporter = true;
    private void Awake()
    {
        GenerateLevel();
        _transportRoomTemp1.OnPlayerInTransporter += RegenerateLevel;
        _transportRoomTemp2.OnPlayerInTransporter += RegenerateLevel;
        _roomTemp.OnPlayerInRoom += StartFight;

    }

    private void OnDisable()
    {
        _transportRoomTemp1.OnPlayerInTransporter -= RegenerateLevel;
        _transportRoomTemp2.OnPlayerInTransporter -= RegenerateLevel;
        _roomTemp.OnPlayerInRoom -= StartFight;
    }

    private void StartFight()
    {
        if (_entityInRoomCountTemp > 0 || _entityListTemp != null)
        {
            return;
        }
        //spawnEnemy && closeDoors
        _transportRoomTemp1.LockDoor();
        _transportRoomTemp2.LockDoor();

        
    }
    private void EndFight()
    { 
        _entityInRoomCountTemp--;
        if (_entityInRoomCountTemp > 0)
        {
            
            return;
        }
        _transportRoomTemp1.UnlockDoor();
        _transportRoomTemp2.UnlockDoor();
    }

    private void SpawnEntity()
    {
        if (_entityListTemp!=null)
        {
            foreach (var _gameObject in _entityListTemp)
            {
                Destroy(_gameObject);
            }
            _entityListTemp.Clear();
        }

        _entityInRoomCountTemp = 0;
        _entityListTemp = new List<GameObject>();
        
        for (int _i = 0; _i < _entityInRoomCount; _i++)
        {
            GameObject _tempObject = Instantiate(_entityList[Random.Range(0 , _entityListTemp.Count)]
                ,_roomTemp.EntitySpawnPoints[Random.Range(0,_roomTemp.EntitySpawnPoints.Count)].position+Vector3.up,quaternion.identity);

            _entityInRoomCountTemp++;
            
            _entityListTemp.Add(_tempObject);
        }

        foreach (var _enemy in Entity.enemyList)
        {
            _enemy.GetComponent<Entity>().OnDie += EndFight; // переделать
        }
    }

    private void GenerateLevel()
    {
        _transportRoomTemp1 = Instantiate(_startRoom[Random.Range(0,_startRoom.Length)]).GetComponent<TransportRoom>();
        _transportRoomTemp2 = Instantiate(_exitRoom[Random.Range(0,_startRoom.Length)]).GetComponent<TransportRoom>();
        _roomTemp = Instantiate(_room[Random.Range(0,_startRoom.Length-1)]).GetComponent<Room>();
        
        _transportRoomTemp1.transform.Rotate(new Vector3(0,-45f,0));
        _transportRoomTemp2.transform.Rotate(new Vector3(0,-45f,0));
        _roomTemp.transform.Rotate(new Vector3(0,-45f,0));
        _navMesh.BuildNavMesh();
        _playerTransform.position = Vector3.up + _transportRoomTemp1.transform.position;

        SpawnEntity();
    }
    
    private void RegenerateLevel()
    {
        _transportRoomTemp1.OnPlayerInTransporter -= RegenerateLevel;
        _transportRoomTemp2.OnPlayerInTransporter -= RegenerateLevel;
        _roomTemp.OnPlayerInRoom -= StartFight;
        
        if (_transporter)
        {
            // room & first transporter
            Destroy(_roomTemp);
            Destroy(_transportRoomTemp1.gameObject);
            
            _roomTemp = Instantiate(_room[Random.Range(0,_startRoom.Length)]).GetComponent<Room>();
            _transportRoomTemp1 = Instantiate(_startRoom[Random.Range(0,_startRoom.Length)]).GetComponent<TransportRoom>();
            
            _transportRoomTemp1.transform.Rotate(new Vector3(0,-45f,0));
            _roomTemp.transform.Rotate(new Vector3(0,-45f,0));

            _transportRoomTemp1.SetIsExit(true);
            _transportRoomTemp2.SetIsExit(false);
            
        }
        else
        {
            // room & second transporter
            Destroy(_roomTemp);
            Destroy(_transportRoomTemp2.gameObject);
            
            _roomTemp = Instantiate(_room[Random.Range(0,_startRoom.Length)]).GetComponent<Room>();
            _transportRoomTemp2 = Instantiate(_exitRoom[Random.Range(0,_startRoom.Length)]).GetComponent<TransportRoom>();
            
            _transportRoomTemp2.transform.Rotate(new Vector3(0,-45f,0));
            _roomTemp.transform.Rotate(new Vector3(0,-45f,0));
            
            _transportRoomTemp1.SetIsExit(false);
            _transportRoomTemp2.SetIsExit(true);
        }
        
        _transportRoomTemp1.OnPlayerInTransporter += RegenerateLevel;
        _transportRoomTemp2.OnPlayerInTransporter += RegenerateLevel;
        _roomTemp.OnPlayerInRoom += StartFight;
        
        _transporter = !_transporter;
        
        SpawnEntity();
        
        _transportRoomTemp1.UnlockDoor();
        _transportRoomTemp2.UnlockDoor();
    }
}
