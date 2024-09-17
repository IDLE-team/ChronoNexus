using UnityEngine;

public class MaterialObject : DropObject
{
    protected override void IncreaseValue()
    {
        GameController.Instance.AddMaterials(Random.Range(5, _maxAmount));
        base.IncreaseValue();
    }
}
