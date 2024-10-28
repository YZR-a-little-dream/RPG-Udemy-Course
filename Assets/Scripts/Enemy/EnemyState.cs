using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    protected bool triggerCalled;           //ÅÐ¶Ï¹¥»÷¶¯»­ÊÇ·ñ²¥·ÅÍê
    private string animBoolName;

    protected float stateTimer;            //×´Ì¬¼ÆÊ±Æ÷

    public EnemyState(Enemy _enemyBase,EnemyStateMachine _stateMachine,string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        //Debug.Log("Update:" + stateMachine.currentState);
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName,true);

        //Debug.Log("Enter:" + stateMachine.currentState);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName,false);

        //Debug.Log("Exit:" + stateMachine.currentState);
    }

    public virtual void AnimationFinishTrigger() => triggerCalled = true;
}
