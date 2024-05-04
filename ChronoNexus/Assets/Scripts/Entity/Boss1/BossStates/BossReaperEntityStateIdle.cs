using UnityEngine;

public class BossReaperEntityStateIdle : BossReaperEntityState
{
    
    public BossReaperEntityStateIdle(EntityBossReaper entity, StateMachine stateMachine):base(entity, stateMachine)
    {
        _entity = entity;
        _stateMachine = stateMachine;
    }
    public override void Enter()
    {
        
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
