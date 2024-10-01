using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _StateMachine, string _animaBoolName) 
        : base(_player, _StateMachine, _animaBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();

        if(xInput != 0)
           stateMachine.ChangeState(player.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
