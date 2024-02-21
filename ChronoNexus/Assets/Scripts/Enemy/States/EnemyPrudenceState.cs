using UnityEngine;

public class EnemyPrudenceState : EnemyState
{
    //ƒл€ дальников: ѕротивник останавливаетс€ на месте и целитс€ в игрока. —корость атаки сильно уменьшена, но урон сильно повышен
    //ƒл€ ближников: ѕротивник становитс€ неу€звимым на 2* секунды, скорость атаки увеличиваетс€


    [SerializeField] private float _immortalityTime = 2f;
    public float ImmortalityTime => _immortalityTime;

    public EnemyPrudenceState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        switch (_enemy.enemyType)
        {
            case Enemy.EnemyType.Stormtrooper:
                //_enemy.EnemyAttacker.MultiplyAttackInterval(2f);
                _enemy.EnemyAttacker.SwapBullet(1);

                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                // понижение скорости атаки
                // замена пули на более сильную
                // enemy attacker
                break;
            case Enemy.EnemyType.Guard:
                _enemy.EnemyAttacker.ActivateImmortality(true);
                //_enemy.EnemyAttacker.MultiplyAttackInterval(0.5f);

                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);

                //анимаци€ падени€

                //_stateMachine.ChangeState(_enemy.ChaseState);
                // активаци€ неу€звимости
                // увеличение скорости атаки
                // enemy attacker
                break;
            default:
#if UNITY_EDITOR
                Debug.Log(this._enemy + " Enemy Prudence State entered without buffs");
#endif
                break;
        }
    }

    public override void LogicUpdate()
    {
        if (_immortalityTime <= 0)
        {
            _enemy.EnemyAttacker.ActivateImmortality(false);
            _stateMachine.ChangeState(_enemy.ChaseState);
        }
        else
        {
            _immortalityTime -= Time.deltaTime;
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void Exit()
    {
        /*_enemy.EnemyAttacker.ActivateImmortality(false);
        _enemy.EnemyAttacker.ResetDamage();
        _enemy.EnemyAttacker.ResetSpeedAttack();*/
    }


}
