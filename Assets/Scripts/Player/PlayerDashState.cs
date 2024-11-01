using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, string _animaBoolName) : base(_player, _playerStateMachine, _animaBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.skill.clone.CreateClone(player.transform);

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0,rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsGroundedDetected() && player.IsWallDeteced())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        player.SetVelocity(player.dashSpeed * player.dashDir,0);

        if(stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    
    
}
