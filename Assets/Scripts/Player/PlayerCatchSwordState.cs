using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animaBoolName) : base(_player, _playerStateMachine, _animaBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        if(player.transform.position.x > sword.position.x && player.facingDir == 1)
        {
            player.Filp();
        }
        else if(player.transform.position.x < sword.position.x && player.facingDir == -1)
        {
            player.Filp();
        }

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir,rb.velocity.y);         //设置剑的回收力
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor",0.1f);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}

