using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class Shelter : MonoBehaviour
{
    [SerializeField] private float timeToSetPosition = 1f;

    private bool _isPlayerInShelter = false;
    private bool _isPlayerInPosition = false;
    private GameObject player;

    [SerializeField] private CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
        ShelterAndRetreat(cancellationTokenSource.Token).Forget();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Start timer
            player = other.gameObject;
            _isPlayerInShelter = true;
            EnterShelter();
        }
    }
    /*private void Update()
    {
        if (_isPlayerInShelter)
        {
            if (Mathf.Abs(player.GetComponent<Rigidbody>().velocity.magnitude) > 0)
            {
                ExitShelter();
            }
            
        }
    }*/
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Start timer
            Debug.Log(player.GetComponent<Rigidbody>().velocity.magnitude);
            if (player.GetComponent<Rigidbody>().velocity.magnitude > 0.05f)
            {
                ExitShelter();
            }
            /*else if (player.GetComponent<Rigidbody>().velocity.magnitude <= 0.05f)
            {
                EnterShelter();
            }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //reset timer and stop corutine
            ExitShelter();
        }
    }

    private void EnterShelter()
    {
        cancellationTokenSource = new CancellationTokenSource();
        ShelterAndRetreat(cancellationTokenSource.Token).Forget();
    }

    private void ExitShelter()
    {
        _isPlayerInShelter = false;
        _isPlayerInPosition = false;
        player.GetComponent<CharacterAnimator>().Sit(false);
        player.GetComponent<Health>()._inShelter = false;
    }

    private async UniTask ShelterAndRetreat(CancellationToken cancellationToken)
    {
        while (_isPlayerInShelter && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int)(timeToSetPosition * 1000));
            if (this == null)
            {
                cancellationTokenSource.Cancel();
            }
            if (_isPlayerInShelter)
            {
                cancellationTokenSource.Cancel();
            }
            if (_isPlayerInPosition)
            {
                cancellationTokenSource.Cancel();
            }
            if (!cancellationToken.IsCancellationRequested)
            {
                Debug.Log("SHELTERED!!!!");

                player.GetComponent<CharacterAnimator>().Sit(true);
                player.transform.DOMove(new Vector3(transform.position.x, player.transform.position.y, transform.position.z), 0.5f);
                player.GetComponent<Health>()._inShelter = true;



                //startanimation
                _isPlayerInPosition = true;
            }
        }
        await UniTask.Yield();
    }
}
