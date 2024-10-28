using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;

    protected Transform player;
    
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) 
    : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //FIXME: 后续会通过玩家管理器（playerManager）来获取位置信息
        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    } 

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected() ||
            Vector2.Distance(enemy.transform.position,player.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}

