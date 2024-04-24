using System;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CopyCameraSettings : MonoBehaviour
{
    public CinemachineVirtualCamera sourceVirtualCamera;
    public Camera targetCamera;


    private void Start()
    {
       // targetCamera.transform.position = sourceVirtualCamera.transform.position; 
      //  targetCamera.transform.rotation = sourceVirtualCamera.transform.rotation;
    }

    private void Update()
    {
        if (sourceVirtualCamera != null && targetCamera != null)
        {
            //targetCamera.transform.position = sourceVirtualCamera.transform.position; 
            //targetCamera.transform.rotation = sourceVirtualCamera.transform.rotation;
            // Копируем позицию и вращение
           // targetCamera.transform.position = sourceVirtualCamera.transform.position; 
           // targetCamera.transform.rotation = sourceVirtualCamera.transform.rotation;

            // Копируем параметры камеры
          //  targetCamera.fieldOfView = sourceVirtualCamera.m_Lens.FieldOfView;
           // targetCamera.farClipPlane = sourceVirtualCamera.m_Lens.FarClipPlane;
             //targetCamera.nearClipPlane = sourceVirtualCamera.m_Lens.NearClipPlane;
            targetCamera.orthographicSize = sourceVirtualCamera.m_Lens.OrthographicSize;
        }
    }
}