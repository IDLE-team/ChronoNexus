using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RecallInteractorVisualizer : MonoBehaviour
{
    [SerializeField] private ObjectPathVisualizer _objectPathVisualizer;
    [SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private Material _standardMaterial;
    [SerializeField] private Material _preparedHighlightMaterial;
    [SerializeField] private Material _activeHighlightMaterial;

    private void OnValidate()
    {
        _meshRenderer ??= GetComponent<MeshRenderer>();
    }

    public void ShowPreparation(int layer)
    {
       // Debug.Log("Layer: " + layer);
       // Debug.Log("GM Layer: " + gameObject.layer);
        //SetLayer(layer);

        _objectPathVisualizer.ShowPreparation();
        if (_preparedHighlightMaterial != null)
            _meshRenderer.material = _preparedHighlightMaterial;
    }

    public void ShowActivation(int layer)
    {
      //  SetLayer(layer);

        _objectPathVisualizer.ShowActivation(layer);
        if(_activeHighlightMaterial != null)
        _meshRenderer.material = _activeHighlightMaterial;
    }

    public void Hide(int layer)
    {
      //  SetLayer(layer);

        _objectPathVisualizer.Hide();
        _meshRenderer.material = _standardMaterial;
    }

   // private void SetLayer(int layer) => gameObject.layer = layer;
}
