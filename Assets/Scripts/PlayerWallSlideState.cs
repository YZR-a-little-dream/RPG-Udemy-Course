using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _playerStateMachine, string _animaBoolName) : base(_player, _playerStateMachine, _animaBoolName)
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if(xInput != 0 && player.facingDir != xInput)
            stateMachine.ChangeState(player.idleState);

        if(yInput <0 )
            rb.velocity = rb.velocity = new Vector2(0,rb.velocity.y);
        else
            rb.velocity = new Vector2(0,rb.velocity.y * .7f);       //使得y轴方向上更慢

        if(player.IsGroundedDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
