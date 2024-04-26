using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public class BridgeExplode : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> bridges = new List<Rigidbody>();
    [SerializeField] private Transform _explodePoint;
    [SerializeField] private GameObject _explode;
    [SerializeField] private float force = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < bridges.Count; i++)
            {
                _explode.SetActive(true);
                bridges[i].constraints = RigidbodyConstraints.None;
                
                Vector3 forceDir = Random.insideUnitSphere.normalized;
                bridges[i].AddForce(forceDir * force, ForceMode.Impulse);
                
            }
        }
    }
}
