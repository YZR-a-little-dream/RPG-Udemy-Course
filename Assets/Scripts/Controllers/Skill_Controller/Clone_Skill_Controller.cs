using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float colorLoosingSpeed;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    private Transform closestEnemy;
    private int facingDir = 1;


    private bool canDuplicateClone;
    private float chanceToDuplicate;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0)
        {
            sr.color = new Color(1,1,1,sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            
            if(sr.color.a < -0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform _newTransform,float _cloneDuration,
                    bool _canAttack,Vector3 _offset,Transform _clonestEnemy,
                    bool _canDuplicateClone,float _chanceToDuplicate)
    {
        if(_canAttack)
        {
            anim.SetInteger("AttackNumber",UnityEngine.Random.Range(1,3));
        }
            

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _clonestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;             //将cloneTimer设置为负数，即直接退出透明度渐变
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position,attackCheckRadius);
        
        foreach(var hit in colliders)
        {
            //FIXME: 连续多次进行反击后，会让facingdir = 1，从而导致找不到攻击目标
            if(hit.GetComponent<Enemy>() != null)
            {
                PlayerManager.instance.player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                if(canDuplicateClone)
                {
                    if(UnityEngine.Random.Range(0,100) < chanceToDuplicate)    //百分之九十九的可能满足条件
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform,new Vector3(0.75f * facingDir,0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if(closestEnemy != null)
        {
            if(transform.position.x >= closestEnemy.position.x )
            {
                
                facingDir = -1;
                transform.Rotate(0,180,0);
            }
        }
    }
}
