using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _playerStateMachine, string _animaBoolName) 
        : base(_player, _playerStateMachine, _animaBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(player.IsWallDeteced())
            stateMachine.ChangeState(player.wallSlideState); 
        
        if(player.IsGroundedDetected())
            stateMachine.ChangeState(player.idleState);

        if(xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput,rb.velocity.y);              
    }
}
