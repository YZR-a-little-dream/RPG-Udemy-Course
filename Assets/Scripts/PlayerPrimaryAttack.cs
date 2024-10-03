using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;                   //攻击组合记数
    private float lastTimerAttacked;            //最后一次攻击的时间
    private float comboWidow = 2;               //连击经过多长时间重置，默认值设为2
    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _playerStateMachine, string _animaBoolName) : base(_player, _playerStateMachine, _animaBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        if(comboCounter >2 || Time.time > lastTimerAttacked + comboWidow)
        {
            comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);

        player.SetVelocity(player.attackMovement[comboCounter].x * player.facingDir,
                            player.attackMovement[comboCounter].y);
        Debug.Log(player.attackMovement[comboCounter] * player.facingDir);
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        
        player.StartCoroutine("BusyFor",.15f);

        comboCounter++;
        lastTimerAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            player.ZeroVelocity();
        }

        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
