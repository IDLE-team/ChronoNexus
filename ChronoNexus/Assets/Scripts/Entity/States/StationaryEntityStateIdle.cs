using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StationaryEntityStateIdle : StationaryEntityState
{

    public StationaryEntityStateIdle(StationaryEntity stationaryEntity, StateMachine stateMachine):base(stationaryEntity, stateMachine)
    {
        
    }

    public override void Enter()
    {
        _stationaryEntity.StartSeek();
        base.Enter();
    }

    public override void LogicUpdate()
    {
        CheckTarget();
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        _stationaryEntity.StopSeek();
        base.Exit();
    }
}