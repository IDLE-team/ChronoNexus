using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTimeStop : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float triggerRadius;
    [SerializeField] private List<ITimeBody> timeBodies = new List<ITimeBody>();
    void Start()
    {

        transform.localScale = new Vector3(triggerRadius, triggerRadius, triggerRadius);
      //  GetComponent<SphereCollider>().radius = triggerRadius;
        ActivateTimeStop();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ITimeBody timeBody ))
        {
            foreach(ITimeBody tbody in timeBodies)
            {
                if(tbody == timeBody)
                {
                    Debug.Log("Enter Return");
                    return;
                }
            }
            Debug.Log("Added");

            timeBodies.Add(timeBody);
            timeBody.SetStopTime();
        }
    }

    private void ActivateTimeStop()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, triggerRadius/2.55f);

        foreach (Collider collider in colliders)
        {
            ITimeBody timeBody = collider.GetComponent<ITimeBody>();
            if (timeBody != null)
            {
                timeBodies.Add(timeBody);
                timeBody.SetStopTime();
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        // Рисуем отладочную сферу для визуализации зоны действия
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius/2.55f);
    }
}
