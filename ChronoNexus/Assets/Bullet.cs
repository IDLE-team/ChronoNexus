using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector3 _shootDir;

    public void SetTarget(Vector3 _shootDir)
    {
        this._shootDir = _shootDir;
    }

    private void Start()
    {
        Destroy(gameObject, 2);
    }

    private void FixedUpdate()
    {
        transform.position += _shootDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out IDamagable target))
        {
            target.TakeDamage(10);
            Destroy(gameObject);
        }
    }
}