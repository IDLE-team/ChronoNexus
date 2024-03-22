using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SettinsPrefabData : MonoBehaviour
{
    [SerializeField] private Volume _postProcessVolume;

    public Volume PostProcessVolume => _postProcessVolume;
}
