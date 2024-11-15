using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AniamtionTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] coliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position,enemy.attackCheckRadius);

        foreach(var hit in coliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }
    
    private void OpenCounterWidow() => enemy.OpenCounterAttackWidow();

    private void CloseCounterWidow() => enemy.CloseCounterAttackWidow();
}
