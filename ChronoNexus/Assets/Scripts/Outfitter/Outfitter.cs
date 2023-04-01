using System.Collections.Generic;
using UnityEngine;

public class Outfitter : MonoBehaviour, IOutfitter
{
    public IWeapon Sword { get; }
    public IWeapon Gun { get; }
}