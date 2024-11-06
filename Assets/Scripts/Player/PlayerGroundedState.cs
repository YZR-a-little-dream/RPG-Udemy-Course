using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _playerStateMachine, string _animaBoolName) 
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

        if(Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(player.blackHole);
        }

        if(Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            stateMachine.ChangeState(player.aimSword);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.counterAttack);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))                        //按住鼠标左键攻击
        {
            stateMachine.ChangeState(player.primaryAttack);
        }

        if(!player.IsGroundedDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundedDetected())
            stateMachine.ChangeState(player.jumpState);
    }

    private bool HasNoSword()
    {
        if(!player.sword)                   //玩家无剑
            return true;
        
        player.sword.GetComponent<Sword_Skill_Controller>().ReturenSword();

        return false;
    }
}
