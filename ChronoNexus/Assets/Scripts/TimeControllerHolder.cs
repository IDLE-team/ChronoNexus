using System.Collections;
using UnityEngine;

public class TimeControllerHolder : MonoBehaviour
{
    public float affectRadius;
    public float stopTimeDelay;
    public void StopTime()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, affectRadius);

        // Перебираем найденные объекты
        foreach (Collider collider in colliders)
        {
            // Пытаемся получить доступ к компоненту MonoBehaviour на объекте
            MonoBehaviour[] scripts = collider.gameObject.GetComponents<MonoBehaviour>();

            // Вызываем нужные методы для каждого скрипта
            foreach (MonoBehaviour script in scripts)
            {
                // Проверяем, что компонент реализует нужный интерфейс или содержит нужный метод
                if (script is ITimeAffected)
                {
             //       ((ITimeAffected)script).StopTimeAction();
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
            // Пытаемся получить доступ к компоненту MonoBehaviour на объекте
            MonoBehaviour[] scripts = collider.gameObject.GetComponents<MonoBehaviour>();

            // Вызываем нужные методы для каждого скрипта
            foreach (MonoBehaviour script in scripts)
            {
                // Проверяем, что компонент реализует нужный интерфейс или содержит нужный метод
                if (script is ITimeAffected)
                {
                    ((ITimeAffected)script).RealTimeAction();
                }
            }
        }
    }
}
