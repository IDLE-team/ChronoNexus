using UnityEngine;

public class EnemyPrudenceState : EnemyState
{


    public EnemyPrudenceState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        switch (_enemy.enemyType)
        {
            case Enemy.EnemyType.Stormtrooper:

                _enemy.EnemyAttacker.SetRangedAttackInterval(2f);
                _enemy.EnemyAttacker.SwapBullet(1);
                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                _enemy.EndMoveAnimation();
                break;
            case Enemy.EnemyType.Guard:
                _enemy.EnemyAttacker.ActivateImmortality(true);
                _enemy.EnemyAttacker.SetMeleeAttackInterval(0.5f);
                _stateMachine.ChangeState(_enemy.ChaseState);
                break;
            default:
                _stateMachine.ChangeState(_enemy.ChaseState);
                break;
        }
    }

    public override void LogicUpdate()
    {
       
    }

    public override void PhysicsUpdate()
    {

    }

    public override void Exit()
    {

    }


}
