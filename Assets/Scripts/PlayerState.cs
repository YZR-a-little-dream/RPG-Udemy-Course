using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    private string animaBoolName;

    protected float stateTimer;                 //¼ÆÊ±Æ÷

    public PlayerState(Player _player,PlayerStateMachine _playerStateMachine,string _animaBoolName)
    {
        this.player = _player;
        this.stateMachine = _playerStateMachine;
        this.animaBoolName = _animaBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animaBoolName , true);
        rb = player.rb;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        
        xInput = Input.GetAxisRaw("Horizontal");
        player.anim.SetFloat("yVelocity",rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animaBoolName , false);
    }
}
