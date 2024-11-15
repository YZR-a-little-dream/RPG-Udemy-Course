using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) 
    : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlick",0,0.1f);

        stateTimer = enemy.stunDuration;

        //此处没有使用SetVelocity避免翻转
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDireciton.x,enemy.stunDireciton.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CannelColorChange",0);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
