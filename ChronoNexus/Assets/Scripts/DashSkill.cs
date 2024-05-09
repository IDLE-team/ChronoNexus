using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class DashSkill : MonoBehaviour, ICoolDownable
{
    [SerializeField] private Character _character;
    [SerializeField] private ParticleSystem _dashVFX;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private List<GameObject> _dashInvisibleGameObjects = new List<GameObject>();
    [SerializeField] private Transform _dashSpawnPoint;
    [SerializeField] private  float _dashSpeed = 20f;
    [SerializeField] private  float _dashTime = 0.2f;
    [SerializeField] private  float _coolDownTime = 2f;

    private PlayerInputActions _input;
    
    
    
    private Rigidbody rb;

    public event Action<float> OnCoolDown;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    [Inject]
    private void Construct(PlayerInputActions input)
    {
        _input = input;
        _input.Player.Dash.performed += OnDash;
    }
    private void OnDestroy()
    {
        _input.Player.Dash.performed -= OnDash;
    }
    private void OnDash(InputAction.CallbackContext obj)
    {
        StartCoroutine(StartDash());
    }
    
    
    IEnumerator StartDash()
    {
        float startTime = Time.time;
        _meshRenderer.enabled = false;
        _character.SetInvincible(true);
        _dashVFX.gameObject.SetActive(true);
        Instantiate(_dashVFX.gameObject, _dashSpawnPoint.position, Quaternion.identity);   
        OnCoolDown?.Invoke(_coolDownTime);
        for (int i = 0; i < _dashInvisibleGameObjects.Count; i++)
        {
            _dashInvisibleGameObjects[i].SetActive(false);
        }
        while (Time.time < startTime + _dashTime)
        {
            rb.velocity = transform.forward * _dashSpeed;
            yield return null;
        }
        _meshRenderer.enabled = true;
        for (int i = 0; i < _dashInvisibleGameObjects.Count; i++)
        {
            _dashInvisibleGameObjects[i].SetActive(true);
        }
        _character.SetInvincible(false);

        Instantiate(_dashVFX.gameObject, _dashSpawnPoint.position, Quaternion.identity);   

        rb.velocity = Vector3.zero;
    }
}

