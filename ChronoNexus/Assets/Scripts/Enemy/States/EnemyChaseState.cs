using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    private Vector3 playerPosition;

    public EnemyChaseState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
        enemy.canSeePlayer = false;
    }

    public override void LogicUpdate()
    {
        if (Vector3.Distance(enemy.transform.position, playerPosition) > 10f)
        {
            enemy.enemySM.ChangeState(enemy.PatrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        playerPosition = enemy.player.position;
        enemy._navMeshAgent.SetDestination(playerPosition);
    }
}