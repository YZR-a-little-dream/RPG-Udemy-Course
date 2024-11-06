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

    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset)
    {
        if(_canAttack)
        {
            anim.SetInteger("AttackNumber",UnityEngine.Random.Range(1,3));
        }
            

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

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
            if(hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,25);

        float closestDistance = math.INFINITY;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
               float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if(closestEnemy != null)
        {
            if(transform.position.x > closestEnemy.position.x )
            {
                transform.Rotate(0,180,0);
            }
        }
    }
}
