using System;
using UnityEngine;

public class EntityBossReaper : Entity
{

    public BossReaperEntityStateFire BossReaperEntityStateFire;
    public BossReaperEntityStateIdle BossReaperEntityStateIdle;

    protected override void InitializeParam()
    {
        base.InitializeParam();
        
        BossReaperEntityStateFire = new BossReaperEntityStateFire(this,_stateMachine);
        BossReaperEntityStateIdle = new BossReaperEntityStateIdle(this,_stateMachine);
    }
    
    
    
    public virtual IState CurrentState
    {
        get { return _stateMachine.CurrentState; }
    }
    protected override void InitializeStartState()
    {
        _stateMachine.Initialize(DummyState);
    }

    public override void TargetFoundReaction(ITargetable target)
    {
        

    }
    public override void TargetLossReaction()
    {
        
    }
    
}
