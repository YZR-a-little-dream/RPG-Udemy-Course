using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;

    public bool isBusy {get; private set;}          //用于解决角色攻击时滑步的问题
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {get;private set;}                     //冲刺方向，避免朝向与冲刺方向
                                                                //的不一致的问题

    #region  States
    public PlayerStateMachine stateMachine {get; private set;}

    public PlayerIdleState idleState {get; private set;}

    public PlayerMoveState moveState {get; private set;}

    public PlayerAirState airState {get; private set;}

    public PlayerJumpState jumpState {get; private set;}

    public PlayerDashState dashState {get; private set;}

    public PlayerWallSlideState wallSlideState{get; private set;}

    public PlayerWallJumpState wallJumpState{get;private set;}  

    public PlayerPrimaryAttackState primaryAttack {get; private set;}

    #endregion

    protected override void Awake() {
        base.Awake();

        stateMachine = new PlayerStateMachine();
 
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this,stateMachine,"Jump");
        dashState = new PlayerDashState(this,stateMachine,"Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine,"Jump");

        primaryAttack = new PlayerPrimaryAttackState(this,stateMachine,"Attack");
    }

    protected override void Start() {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update() {
        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();
    }
    
    //用来处理状态占用问题，避免可以滑步攻击的发生
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
    
        yield return new WaitForSeconds(_seconds);
        
        isBusy = false;
    }

    //如果完成动画将trigger置真
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        dashUsageTimer -= Time.deltaTime;

        if(IsWallDeteced())
           return;

        if(Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0) 
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if(dashDir == 0)
            {   
                dashDir = facingDir;
            }
            
            stateMachine.ChangeState(dashState);
        }
    }
}
