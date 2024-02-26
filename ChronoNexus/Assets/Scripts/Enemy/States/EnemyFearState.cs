using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyFearState : EnemyState
{

    private CancellationTokenSource cancellationTokenSource;
    public EnemyFearState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        cancellationTokenSource = new CancellationTokenSource();
        AttackAndRetreat(cancellationTokenSource.Token).Forget();
    }

    public override void LogicUpdate()
    {
        
    }


    public override void Exit()
    {
        
    }

    public override void PhysicsUpdate()
    {
    }
    private async UniTask AttackAndRetreat(CancellationToken cancellationToken)
    {
        /*while (_isAttack && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int)(Random.Range(minDelay, maxDelay) * 1000));
            if (_enemy == null)
                cancellationTokenSource.Cancel();
            if (_enemy.Target == null)
                cancellationTokenSource.Cancel();

            if (!cancellationToken.IsCancellationRequested)
            {
                Vector3 randomDirection = Random.onUnitSphere.normalized;
                Vector3 retreatPosition = _target.position + randomDirection * retreatDistance;

                retreatPosition = new Vector3(
                    retreatPosition.x,
                    _enemy.transform.position.y,
                    retreatPosition.z
                );

                if (Vector3.Distance(_enemy.transform.position, retreatPosition) > 0.1f)
                {
                    _enemy.NavMeshAgent.SetDestination(retreatPosition);
                    _enemy.StartMoveAnimation();

                    await UniTask.Yield();
                }
                else
                {
                    _enemy.EndMoveAnimation();
                }
            }
        }*/
        await UniTask.Yield();
    }
}
