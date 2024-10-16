using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using NavMeshSurface = Unity.AI.Navigation.NavMeshSurface;
using Random = UnityEngine.Random;

public class InfiniteLevelController : MonoBehaviour
{ 
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject _winGameObject;
    [Header("Lift parameters")]
    [SerializeField] private GameObject[] _lift;
    [SerializeField] private float _moveTime = 7f;
    [SerializeField] private float _moveY = 10f;
    [Header("Rooms parameters")]
    [SerializeField] private GameObject[] _room;
    
    
    [Header("Entity parameters")]
    [SerializeField] private List<GameObject> _entityList;
    [SerializeField] private int _entityInRoomCount = 3;
    [SerializeField] private float _entitySpawnRadius = 2f;
    [SerializeField] private NavMeshSurface _navMesh;
    [SerializeField] private EntityGroupManager _entityGroupManager;
    
    private TransportRoom _transportRoomTemp1;
    private TransportRoom _transportRoomTemp2;
    private Room _roomTemp;
    private int _entityInRoomCountTemp;
    private GameObject _winTemp;
    private int _stageNumber = 0;
    
    [SerializeField]private List<Entity> _entityListTemp;

    private bool _transporter = true;
    private void Awake()
    {
        GenerateLevel();
        _transportRoomTemp1.OnPlayerInTransporter += StartLoadProcess;
        _transportRoomTemp2.OnPlayerInTransporter += StartLoadProcess;
        _roomTemp.OnPlayerInRoom += StartFight;

    }

    private void OnDisable()
    {
        _transportRoomTemp1.OnPlayerInTransporter -= StartLoadProcess;
        _transportRoomTemp2.OnPlayerInTransporter -= StartLoadProcess;
        _roomTemp.OnPlayerInRoom -= StartFight;
    }

    private void StartFight()
    {
        
        _transportRoomTemp1.LockDoor();
        _transportRoomTemp2.LockDoor();
        EnableEntity();


    }
    private void EndFight()
    { 
        _entityInRoomCountTemp--;
        if (_entityInRoomCountTemp != 0)
        {
            return;
        }

        if (_winTemp!=null)
        {
            _winTemp.SetActive(true);
        }
        
        _transportRoomTemp1.UnlockDoor();
        _transportRoomTemp2.UnlockDoor();
    }

    private void EnableEntity()
    {
        foreach (var _enemy in Entity.enemyList)
        {
            _enemy.SetActive(true);
        }
    }

    private void SpawnEntity()
    {
        if (_entityListTemp != null)
        {
            foreach (var _gameObject in _entityListTemp)
            {
                Destroy(_gameObject.transform.parent.gameObject,0.1f);
            }
        }

        _entityInRoomCountTemp = 0;
        _entityListTemp = new List<Entity>();
        

        for (int _i = 0; _i < _entityInRoomCount; _i++)
        {
            Vector3 randomVector = new Vector3(Random.Range(-_entitySpawnRadius,_entitySpawnRadius),0,Random.Range(-_entitySpawnRadius,_entitySpawnRadius));
            GameObject _tempObject = Instantiate(_entityList[Random.Range(0 , _entityList.Count)]
                ,_roomTemp.EntitySpawnPoints[Random.Range(0,_roomTemp.EntitySpawnPoints.Count)].position+Vector3.up+randomVector,quaternion.identity);

            _entityInRoomCountTemp++;
            
            _entityListTemp.Add(_tempObject.GetComponentInChildren<Entity>());
            
        }

        _entityGroupManager.CreateGroup(_entityListTemp);

        foreach (var _enemy in Entity.enemyList)
        {
            _enemy.GetComponent<Entity>().OnDie += EndFight;
            _enemy.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartLoadProcess();
        }
    }

    private void GenerateLevel()
    {
        
        _transportRoomTemp1 = Instantiate(_lift[Random.Range(0,_lift.Length)]).GetComponent<TransportRoom>();
        _transportRoomTemp2 = Instantiate(_lift[Random.Range(0,_lift.Length)]).GetComponent<TransportRoom>();
        _roomTemp = Instantiate(_room[Random.Range(0,_room.Length)]).GetComponent<Room>();

        _roomTemp.transform.rotation = _transportRoomTemp1.Connector.rotation * Quaternion.Inverse(_roomTemp.Connectors[0].localRotation);
        _roomTemp.transform.position = _transportRoomTemp1.Connector.position - (_roomTemp.Connectors[0].position-_roomTemp.transform.position);

        _transportRoomTemp2.transform.rotation = _roomTemp.Connectors[1].rotation * Quaternion.Inverse(_transportRoomTemp2.transform.localRotation);
        _transportRoomTemp2.transform.position =  _roomTemp.Connectors[1].position - ( _transportRoomTemp2.Connector.position - _transportRoomTemp2.transform.position);
        
            _winTemp = Instantiate(_winGameObject, _roomTemp.WinGameTransform.position + Vector3.up,Quaternion.identity);
            _winTemp.SetActive(false);
        
        _transportRoomTemp2.SetIsExit(true);
        _navMesh.RemoveData();
        _navMesh.BuildNavMesh();
        _playerTransform.position = Vector3.up + _transportRoomTemp1.transform.position;

        SpawnEntity();
    }

    private void StartLoadProcess()
    {
        _navMesh.RemoveData();
        
        
        LevelController.instance.LoadProcessWithTransition(2f,true);
        _stageNumber += 1;
        
        if (_transporter)
        {
            _transportRoomTemp2.transform.DOMoveY(_transportRoomTemp2.transform.position.y + _moveY, _moveTime).OnComplete(() =>
            {
                RegenerateLevel();
                LevelController.instance.LoadProcessWithTransition(2f,false);
            });
        }
        else
        {
            _transportRoomTemp1.transform.DOMoveY(_transportRoomTemp1.transform.position.y + _moveY, _moveTime).OnComplete(() =>
            {
                RegenerateLevel();
                LevelController.instance.LoadProcessWithTransition(2f,false);
            });
        }
        
    }
    
    private void RegenerateLevel()
    {
        _transportRoomTemp1.OnPlayerInTransporter -= StartLoadProcess;
        _transportRoomTemp2.OnPlayerInTransporter -= StartLoadProcess;
        _roomTemp.OnPlayerInRoom -= StartFight;
        
        if (_transporter)
        {
            // room & first transporter
            Destroy(_roomTemp.gameObject);
            Destroy(_transportRoomTemp1.gameObject);
            
            _roomTemp = Instantiate(_room[Random.Range(0,_room.Length)]).GetComponent<Room>();
            _transportRoomTemp1 = Instantiate(_lift[Random.Range(0,_lift.Length)]).GetComponent<TransportRoom>();

            _roomTemp.transform.rotation = _transportRoomTemp2.Connector.rotation * Quaternion.Inverse(_roomTemp.Connectors[1].localRotation);
            _roomTemp.transform.position = _transportRoomTemp2.Connector.position - (_roomTemp.Connectors[1].position - _roomTemp.transform.position) ;

            _transportRoomTemp1.transform.rotation = _roomTemp.Connectors[0].rotation * Quaternion.Inverse(_transportRoomTemp1.transform.localRotation);
            _transportRoomTemp1.transform.position =_roomTemp.Connectors[0].position - ( _transportRoomTemp1.Connector.position - _transportRoomTemp1.transform.position); ;
            

            _transportRoomTemp1.SetIsExit(true);
            _transportRoomTemp2.SetIsExit(false);
            _playerTransform.position = Vector3.up + _transportRoomTemp2.transform.position;
        }
        else
        {
            // room & second transporter
            Destroy(_roomTemp.gameObject);
            Destroy(_transportRoomTemp2.gameObject);
            
            _roomTemp = Instantiate(_room[Random.Range(0,_room.Length)]).GetComponent<Room>();
            _transportRoomTemp2 = Instantiate(_lift[Random.Range(0,_lift.Length)]).GetComponent<TransportRoom>();

            _roomTemp.transform.rotation = _transportRoomTemp1.Connector.rotation * Quaternion.Inverse(_roomTemp.Connectors[0].localRotation);
            _roomTemp.transform.position = _transportRoomTemp1.Connector.position - (_roomTemp.Connectors[0].position-_roomTemp.transform.position);

            _transportRoomTemp2.transform.rotation = _roomTemp.Connectors[1].rotation * Quaternion.Inverse(_transportRoomTemp2.transform.localRotation);
            _transportRoomTemp2.transform.position =  _roomTemp.Connectors[1].position - ( _transportRoomTemp2.Connector.position - _transportRoomTemp2.transform.position);
            
            _transportRoomTemp1.SetIsExit(false);
            _transportRoomTemp2.SetIsExit(true);
            _playerTransform.position = Vector3.up + _transportRoomTemp1.transform.position;
        }

        if (_stageNumber == 0 || _stageNumber %10 == 0)
        {
            _winTemp = Instantiate(_winGameObject, _roomTemp.WinGameTransform.position + Vector3.up,Quaternion.identity);
            _winTemp.SetActive(false);
        }
        
        _transportRoomTemp1.OnPlayerInTransporter += StartLoadProcess;
        _transportRoomTemp2.OnPlayerInTransporter += StartLoadProcess;
        _roomTemp.OnPlayerInRoom += StartFight;
        
        _transporter = !_transporter;
        
        _navMesh.BuildNavMesh();
        SpawnEntity(); 
        _transportRoomTemp1.UnlockDoor();
        _transportRoomTemp2.UnlockDoor();
    }
}
