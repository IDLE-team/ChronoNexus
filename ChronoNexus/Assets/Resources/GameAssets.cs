using System;
using UnityEngine;
using System.Reflection;

public class GameAssets : MonoBehaviour
{

    private static GameAssets _i;

    private void Awake()
    {
        _i = this;
    }

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets")) as GameAssets;
            return _i;
        }
    }

    public GameObject pfDamagePopup;
}
