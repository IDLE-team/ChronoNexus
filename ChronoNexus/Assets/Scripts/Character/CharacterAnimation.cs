using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [HideInInspector] public int animIDStrafe { get; private set; }
    [HideInInspector] public int animIDStrafeX { get; private set; }
    [HideInInspector] public int animIDStrafeZ { get; private set; }
    [HideInInspector] public int animIDGrounded { get; private set; }
    [HideInInspector] public int animIDJump { get; private set; }
    [HideInInspector] public int animIDFreeFall { get; private set; }
    [HideInInspector] public int animIDMotionSpeed { get; private set; }
    [HideInInspector] public int animIDTurn { get; private set; }
    [HideInInspector] public int animIDAttack { get; private set; }
    [HideInInspector] public int animIDShoot { get; private set; }

    private void Start()
    {
        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        animIDStrafe = Animator.StringToHash("Strafe");
        animIDStrafeX = Animator.StringToHash("StrafeX");
        animIDStrafeZ = Animator.StringToHash("StrafeZ");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        animIDTurn = Animator.StringToHash("Turn");
        animIDAttack = Animator.StringToHash("Attack");
        animIDShoot = Animator.StringToHash("Shoot");
    }
}