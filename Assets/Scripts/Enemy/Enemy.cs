using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask wharIsPlayer;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake() {
        base.Awake();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Update() {
        base.Update();
        
        stateMachine.currentState.Update();
    }

    public virtual void AnimationFinishTrigger() =>
        stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() 
        => Physics2D.Raycast(wallCheck.position,Vector2.right * facingDir,50,wharIsPlayer);

    //ÖØÐ´¹ÖÎï¹¥»÷·¶Î§
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
        new Vector3(transform.position.x + attackDistance * facingDir,
        transform.position.y));
    }
}
