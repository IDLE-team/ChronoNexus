using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BossReaperEntityStateFire : BossReaperEntityState
{
    private bool _isAttack = false;
    private float shootingTimer = 0;
    private float shootingInterval;

    private float retreatDistance = 1f;

    private float reloadTimer = 0;
    private float reloadInterval = 3f;

    private float _setTargetOnAim = 0.5f;
    private float _setTargetOnAimTemp = 0.5f;

    private bool _isReloading = false;

    private int ammoCount;
    private int ammoMaxCount;
    
    private Quaternion _toRotation;
    
    
    public BossReaperEntityStateFire(EntityBossReaper entity, StateMachine stateMachine):base(entity, stateMachine)
    {
        _entity = entity;
        _stateMachine = stateMachine;
    }
    
    public override void Enter()
    {

        _entity.TargetFinder.SetWeight(1);
        _isAttack = true;

        base.Enter();
    }

    public override void LogicUpdate()
    {
        if (_entity.isTimeSlowed)
        {
            if (!_isReloading)
            {
                _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation,
                    CalculateRotation(), 6f * 0.2f * Time.deltaTime);
            }

        }
        else
        {
            if (!_isReloading)
            {
                _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation,
                    CalculateRotation(), 6f * Time.deltaTime);
            }
        }
        
        ShootLogic();
        ReloadLogic();
    }
    
    public override void PhysicsUpdate()
    {
       
    }
    
    public override void Exit()
    {
        
    }
    
    private void ShootLogic()
    {
        if (!_isReloading)
        {
            if (_entity.isTimeSlowed)
            {
                shootingTimer -= (Time.deltaTime * 0.2f);
            }
            else
            {
                shootingTimer -= Time.deltaTime;
            }

        }

        if (shootingTimer <= 0f && !_isReloading)
        {
            _entity.BossReaperAttacker.Shoot(_entity.Target.GetTransform().position);
            shootingTimer = shootingInterval;
            if (ammoCount > 0)
            {
                ammoCount--;
            }
            else if (!_isReloading)
            {
                ammoCount = ammoMaxCount;
                reloadTimer = reloadInterval;
                //start reloading animation
                _isReloading = true;
            }
        }
    }

    private void ReloadLogic()
    {
        if (reloadTimer >= 0f && _isReloading)
        {
            if (_entity.isTimeSlowed)
            {
                reloadTimer -= (Time.deltaTime * 0.2f);
            }
            else
            {
                reloadTimer -= Time.deltaTime;
            }
        }
        else if (_isReloading)
        {
            _isReloading = false;
        }
    }
    
    private Quaternion CalculateRotation()
    {
        Quaternion toRotation = Quaternion.LookRotation(_entity.Target.GetTransform().position - _entity.SelfAim.position, Vector3.up);
        toRotation.x = 0f;
        toRotation.z = 0f;
        return toRotation;
    }
    
    protected virtual async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_entity.isTimeSlowed && !_entity.isTimeStopped);
    }
}
