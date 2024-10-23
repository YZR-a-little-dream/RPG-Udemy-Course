using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack details")]
    public Vector2[] attackMovement;

    public bool isBusy {get; private set;}          //���ڽ����ɫ����ʱ����������
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {get;private set;}                     //��̷��򣬱��⳯�����̷���
                                                                //�Ĳ�һ�µ�����
    #region  Collidion info
    [Header("Collision info")]                                                          
    [SerializeField] private Transform groundCheck;             //������
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;               //ǽ�ڼ��
    [SerializeField] private float wallCheckDistance;   
    [SerializeField] private LayerMask whatIsGround;            //׼�����߼���layerMask
    #endregion

    public int facingDir {get; private set;} = 1;               //����泯�ķ���
    private bool facingRight = true;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    
    #endregion

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

    private void Awake() {
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

    private void Start() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    private void Update() {
        stateMachine.currentState.Update();

        CheckForDashInput();
    }
    
    //��������״̬ռ�����⣬������Ի��������ķ���
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
    
        yield return new WaitForSeconds(_seconds);
        
        isBusy = false;
    }

    //�����ɶ�����trigger����
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
    #region  Velocity
    public void ZeroVelocity() => rb.velocity = Vector2.zero;

    public void SetVelocity(float _xVelocity,float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FilpController(_xVelocity);
    }
    #endregion

    #region Collision  
    public bool IsGroundedDetected() => 
        Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);

    public bool IsWallDeteced() =>
        Physics2D.Raycast(wallCheck.position,Vector2.right * facingDir,wallCheckDistance,whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x 
            + wallCheckDistance,wallCheck.position.y));
    }
    #endregion

    #region  Filp
    public void Filp()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FilpController(float _x)
    {
        if(_x > 0 && !facingRight)
        {
            Filp();
        }else if(_x < 0 && facingRight)
        {
            Filp();
        }
    }
    #endregion
}