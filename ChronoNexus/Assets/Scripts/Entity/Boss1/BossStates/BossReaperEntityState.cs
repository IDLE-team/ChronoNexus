using UnityEngine;

public class BossReaperEntityState : IState
{
    protected EntityBossReaper _entity;
    protected StateMachine _stateMachine;

    protected BossReaperEntityState(EntityBossReaper entity, StateMachine stateMachine)
    {
        _entity = entity;
        _stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
        
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
       
    }

    public virtual void Exit()
    {
        
    }
}
