using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;           //找到玩家坐标
    private Enemy_Skeleton enemy;
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) 
    : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if(canAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            //追击时间少于0 或者超过追击距离
            if(stateTimer < 0 || Vector2.Distance(player.transform.position,enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        if(player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }else if(player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir,rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool canAttack()
    {
        if(Time.time > enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        //Debug.Log("Attack is on cooldown");
        return false;
    }
}
