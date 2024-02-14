using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecallConfig", menuName = "Skills/RecallConfig")]
public class RecallConfig : ScriptableObject
{
    [SerializeField, Range(0, 10)] private float _secondsToRecord;
    [SerializeField, Range(0, 10)] private float _radius;
    [SerializeField] private string _skillInteractableLayerName;
    [SerializeField] private string _timeRewindLayerName;

    public float SecondsToRecord => _secondsToRecord;
    public LayerMask SkillInteractableLayerMask => 1 << LayerMask.NameToLayer(_skillInteractableLayerName);

    public int SkillInteractableLayer => LayerMask.NameToLayer(_skillInteractableLayerName);

    public int TimeRewindLayer => LayerMask.NameToLayer(_timeRewindLayerName);

    public float Radius => _radius;


}
