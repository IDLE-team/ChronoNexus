using UnityEngine;

public class EnemyPrudenceState : EnemyState
{
    //��� ���������: ��������� ��������������� �� ����� � ������� � ������. �������� ����� ������ ���������, �� ���� ������ �������
    //��� ���������: ��������� ���������� ���������� �� 2* �������, �������� ����� �������������


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
                // ��������� �������� �����
                // ������ ���� �� ����� �������
                // enemy attacker
                break;
            case Enemy.EnemyType.Guard:
                _enemy.EnemyAttacker.ActivateImmortality(true);
                //_enemy.EnemyAttacker.MultiplyAttackInterval(0.5f);

                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);

                //�������� �������

                //_stateMachine.ChangeState(_enemy.ChaseState);
                // ��������� ������������
                // ���������� �������� �����
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
