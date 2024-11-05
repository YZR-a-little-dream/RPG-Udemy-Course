using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask wharIsPlayer;

    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunDireciton;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;     //反击时的红色方块

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    public float defaultMoveSpeed;                          //保留冻结前的移动速度

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake() {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update() {
        base.Update();
        
        stateMachine.currentState.Update();
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if(_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWidow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWidow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanbeStunned()
    {
        if(canBeStunned)
        {
            CloseCounterAttackWidow();
            return true;
        }
        return false;
    }

    public virtual void AnimationFinishTrigger() =>
        stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() 
        => Physics2D.Raycast(wallCheck.position,Vector2.right * facingDir,50,wharIsPlayer);

    //重写怪物攻击范围
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
        new Vector3(transform.position.x + attackDistance * facingDir,
        transform.position.y));
    }
}
