using Cysharp.Threading.Tasks;

public class EnemySoldierState : EnemyState
{
    protected new EnemySoldier _enemy;
    private float _defaultAgentSpeed = 1.5f;
    public EnemySoldierState(EnemySoldier enemy, StateMachine stateMachine) : base(enemy,stateMachine)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        if (!_enemy.isTimeSlowed && !_enemy.isTimeStopped)
        {
            //_enemy.NavMeshAgent.speed *= 2;
            _enemy.NavMeshAgent.speed = _defaultAgentSpeed;
        }
        else
        {
            TimeWaiter().Forget();
        }
    }

    public override void LogicUpdate()
    {
    }

    public override void Exit()
    {
        _enemy.StopSeek();
    }

    public override void PhysicsUpdate()
    {
        
    }
    protected virtual async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_enemy.isTimeSlowed && !_enemy.isTimeStopped);
        //_enemy.NavMeshAgent.speed *= 2;
        _enemy.NavMeshAgent.speed = _defaultAgentSpeed;
    }
}