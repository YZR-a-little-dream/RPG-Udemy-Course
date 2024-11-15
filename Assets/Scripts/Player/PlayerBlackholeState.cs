using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;          //设置玩家腾空时间
    private bool skillUsed;

    private float defaultGravity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _playerStateMachine, string _animaBoolName) : base(_player, _playerStateMachine, _animaBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.fx.Maketransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
            rb.velocity = new Vector2(0,15);            //让玩家上升
        
        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0,-0.1f);
            if(!skillUsed)
            {
                if(player.skill.blackhole.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }

        //we exit state in blackhole skills controller when all of the attack are over

        if(player.skill.blackhole.SkillCompleted())
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
