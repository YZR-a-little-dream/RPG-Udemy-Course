using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;      //反击持续时间

    public bool isBusy {get; private set;}          //用于解决角色攻击时滑步的问题
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float swordReturnImpact;                 //剑回收的反作用力
    public float defaultMoveSpeed;
    public float defaultJumpForce;


    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir {get;private set;}         //冲刺方向，避免朝向与冲刺方向的不一致的问题

    public SkillManager skill {get; private set;}
    public GameObject sword {get; private set;}

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
    public PlayerCounterAttackState counterAttack {get; private set;}
    public PlayerAimSwordState aimSword {get; private set;}
    public PlayerCatchSwordState catchSword {get; private set;}
    public PlayerBlackholeState blackHole {get; private set;}
    public PlayerDeadState deadState {get; private set;}
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
        counterAttack = new PlayerCounterAttackState(this,stateMachine,"CounterAttack");

        aimSword = new PlayerAimSwordState(this,stateMachine,"AimSword");
        catchSword = new PlayerCatchSwordState(this,stateMachine,"CatchSword");
        blackHole = new PlayerBlackholeState(this,stateMachine,"Jump");

        deadState = new PlayerDeadState(this,stateMachine,"Die");
    }

    protected override void Start() {
        base.Start();
        
        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update() {
        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();

        if(Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }
    }

    public override void SlowEntityby(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("RetuenDefaultSpeed",_slowDuration);
    }

    protected override void RetuenDefaultSpeed()
    {
        base.RetuenDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }


    public void AssignNewSword(GameObject _newSword) 
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
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
        
        if(IsWallDeteced())
           return;

        if(Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill()) 
        {
            
            dashDir = Input.GetAxisRaw("Horizontal");

            if(dashDir == 0)
            {   
                dashDir = facingDir;
            }
            
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
