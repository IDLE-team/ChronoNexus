using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class AimRigController : MonoBehaviour
{
    [SerializeField] private Rig _rig;
    public Transform _aimTarget;

    [SerializeField] private List<MultiAimConstraint> _constraints = new List<MultiAimConstraint>();
    public void SetWeight(int weight)
    {
        _rig.weight = weight;
    }

   //public void SetConstraintsTarget()
   // {

   // }


}
