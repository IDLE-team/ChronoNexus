using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinDataManager : MonoBehaviour
{
    public static SkinDataManager skinManager;
    [SerializeField] private List<SkinData> _allGameSkins = new List<SkinData>();

    private void Awake()
    {
        if (!skinManager)
        {
            skinManager = this;
        }
        else if (skinManager == this)
        {
            Destroy(gameObject);
        }
    }


    public int GetIndexBySkinData(SkinData item)
    {
        return _allGameSkins.IndexOf(item);
    }

    public SkinData GetSkinDataByIndex(int index)
    {
        return _allGameSkins[index];
    }

    public List<SkinData> GetAllGameSkins()
    {
        return _allGameSkins;
    }

    public int GetSkinListCount()
    {
        return _allGameSkins.Count;
    }
}
