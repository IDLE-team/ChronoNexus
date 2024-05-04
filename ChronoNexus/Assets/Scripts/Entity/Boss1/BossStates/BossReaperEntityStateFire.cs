using UnityEngine;

public class BossReaperEntityStateFire : BossReaperEntityState
{
    private float _attackTimer = 2f;
    public BossReaperEntityStateFire(EntityBossReaper entity, StateMachine stateMachine):base(entity, stateMachine)
    {
        _entity = entity;
        _stateMachine = stateMachine;
    }
    public override void Enter()
    {
        
    }

    public override void LogicUpdate()
    {
        ShootLogic();
        ReloadLogic();
    }
    
    private void ShootLogic()
    {
       
    }
    
    private void ReloadLogic()
    {
        
    }
    

    public override void PhysicsUpdate()
    {
       
    }

    public override void Exit()
    {
        
    }
}
