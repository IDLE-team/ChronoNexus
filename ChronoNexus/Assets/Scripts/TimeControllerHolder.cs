using System.Collections;
using UnityEngine;

public class TimeControllerHolder : MonoBehaviour
{
    public float affectRadius;
    public float stopTimeDelay;
    public void StopTime()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, affectRadius);

        // ���������� ��������� �������
        foreach (Collider collider in colliders)
        {
            // �������� �������� ������ � ���������� MonoBehaviour �� �������
            MonoBehaviour[] scripts = collider.gameObject.GetComponents<MonoBehaviour>();

            // �������� ������ ������ ��� ������� �������
            foreach (MonoBehaviour script in scripts)
            {
                // ���������, ��� ��������� ��������� ������ ��������� ��� �������� ������ �����
                if (script is ITimeAffected)
                {
                    ((ITimeAffected)script).StopTimeAction();
                }
            }
        }
        StartCoroutine(ResumeTime(stopTimeDelay, colliders));
    }


    IEnumerator ResumeTime(float delay, Collider[] timeBodies)
    {
        yield return new WaitForSeconds(delay);
        foreach (Collider collider in timeBodies)
        {
            // �������� �������� ������ � ���������� MonoBehaviour �� �������
            MonoBehaviour[] scripts = collider.gameObject.GetComponents<MonoBehaviour>();

            // �������� ������ ������ ��� ������� �������
            foreach (MonoBehaviour script in scripts)
            {
                // ���������, ��� ��������� ��������� ������ ��������� ��� �������� ������ �����
                if (script is ITimeAffected)
                {
                    ((ITimeAffected)script).RealTimeAction();
                }
            }
        }
    }
}
