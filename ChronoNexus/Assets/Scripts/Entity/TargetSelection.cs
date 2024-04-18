using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class TargetSelection : MonoBehaviour
{
    private MeshRenderer _meshRender;

    private void Awake()
    {
        _meshRender = GetComponent<MeshRenderer>();
        Deselect();
    }

    public void Select()
    {
        _meshRender.enabled = true;
    }

    public void Deselect()
    {
        _meshRender.enabled = false;
    }
}
