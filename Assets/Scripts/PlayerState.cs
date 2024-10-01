using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animaBoolName;

    protected float stateTimer;                 //计时器

    protected bool triggerCalled;               //用来记录攻击是否完成

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
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity",rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animaBoolName , false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
