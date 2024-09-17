using UnityEngine;

public class MaterialDrop : MonoBehaviour
{
    [SerializeField] private GameObject _dropPrefab;
    [SerializeField] private Transform _dropSpawnTransform;
    [SerializeField] private int _maxDropCount;
    public float force = 50f;
    private void Start()
    {
        GetComponent<Entity>().OnDie += DropBuff;
    }
    public void DropBuff()
    {
        int dropCount = Random.Range(1, _maxDropCount);
        for (int i = 0; i < dropCount; i++)
        {

            var buff = Instantiate(_dropPrefab, _dropSpawnTransform.position, Quaternion.identity);

            Rigidbody rb = buff.GetComponent<Rigidbody>();


            Vector3 forceDir = Random.insideUnitSphere.normalized;

            rb.AddForce(forceDir * force, ForceMode.Impulse);
        }
    }
}
