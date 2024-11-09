using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{   
    //解决反击多个多个目标时，会产生多个克隆体攻击敌人
    private bool canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animaBoolName) : base(_player, _playerStateMachine, _animaBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        
        canCreateClone = true;

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack",false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position,player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().CanbeStunned())
                {
                    stateTimer = 10;            //any value bigger than 1
                    player.anim.SetBool("SuccessfulCounterAttack",true);

                    if(canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.clone.CreateCloneOnCounterAttack(hit.transform);
                    }
                    //Debug.Log("SuccessfulCounterAttack");
                }
            }
        }

        if(stateTimer <0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
