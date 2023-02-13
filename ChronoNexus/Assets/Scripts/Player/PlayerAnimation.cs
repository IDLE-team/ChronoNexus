using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [HideInInspector] public int animIDStrafe;
    [HideInInspector] public int animIDStrafeX;
    [HideInInspector] public int animIDStrafeZ;
    [HideInInspector] public int animIDGrounded;
    [HideInInspector] public int animIDJump;
    [HideInInspector] public int animIDFreeFall;
    [HideInInspector] public int animIDMotionSpeed;
    [HideInInspector] public int animIDTurn;
    [HideInInspector] public int animIDAttack;

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
    }
}