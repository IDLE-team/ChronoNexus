using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShopHolder : MonoBehaviour
{
    [SerializeField] private GameObject _skinCellObject;

    private void Start()
    {
        var list = SkinDataManager.skinManager.GetAllGameSkins();

        foreach (var item in list)
        {
            var g = Instantiate(_skinCellObject, transform);
            g.GetComponentInParent<SkinCell>().SetSkinCell(item);
        }
    }
}
