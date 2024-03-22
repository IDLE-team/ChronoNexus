using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPrefabData : MonoBehaviour
{
   [SerializeField] private MainButtonController _mainButtonController;

   public MainButtonController MainButtonController => _mainButtonController;
}
