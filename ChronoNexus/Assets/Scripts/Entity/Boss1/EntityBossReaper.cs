using System;
using UnityEngine;

public class EntityBossReaper : Entity
{
    private EntityBossReaperAttacker _bossReaperAttacker;
    public EntityBossReaperAttacker BossReaperAttacker => _bossReaperAttacker;
    
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
        _stateMachine.Initialize(BossReaperEntityStateFire);
    }

    public override void TargetFoundReaction(ITargetable target)
    {
        

    }
    public override void TargetLossReaction()
    {
        
    }
    
}
