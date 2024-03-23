using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPrefabData : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Camera _cam;
    
    public Character Character => _character;

}
