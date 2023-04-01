using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Selection : MonoBehaviour
{
    private MeshRenderer _meshRender;

    private void Awake()
    {
        _meshRender = GetComponent<MeshRenderer>();
    }

    private void Start() => Deselect();


    public void Select()
    {
        _meshRender.enabled = true;
    }

    public void Deselect()
    {
        _meshRender.enabled = false;
    }
}