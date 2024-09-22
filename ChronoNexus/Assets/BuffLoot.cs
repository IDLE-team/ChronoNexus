using UnityEngine;

public class BuffLoot : MonoBehaviour
{
    [SerializeField] private GameObject _healBuff;
    [SerializeField] private Transform _buffSpawnTransform;
    [SerializeField] private int _maxBuffDropCount;
    public float force = 50f;
    public void DropBuff()
    {
        int dropCount = Random.Range(1, 5);
        for (int i = 0; i < dropCount; i++)
        {

            var buff = Instantiate(_healBuff, _buffSpawnTransform.position, Quaternion.identity);

            Rigidbody rb = buff.GetComponent<Rigidbody>();


            Vector3 forceDir = Random.insideUnitSphere.normalized;

            rb.AddForce(forceDir * force, ForceMode.Impulse);
        }
    }
}
