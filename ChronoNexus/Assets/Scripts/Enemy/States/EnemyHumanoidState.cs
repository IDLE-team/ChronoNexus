using Cysharp.Threading.Tasks;

public class EnemyHumanoidState : EnemyState
{
    protected EnemyHumanoid _enemy;
    private float _defaultAgentSpeed = 1.5f;
    public EnemyHumanoidState(EnemyHumanoid enemy, StateMachine stateMachine) : base(stateMachine)
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