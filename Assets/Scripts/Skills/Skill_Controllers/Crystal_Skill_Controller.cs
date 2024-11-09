using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;           //±¬Õ¦Ë²¼ä·Å´ó
    private float growSpeed = 5;   

    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;

    public void SetupCryStal(float _crystalExistTimer, bool _canExplode, bool _canMove, float _moveSpeed,Transform _closestTarget)
    {
        crystalExistTimer = _crystalExistTimer;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,radius,whatIsEnemy);
        
        if(colliders.Length > 0)
            closestTarget = colliders[UnityEngine.Random.Range(0,colliders.Length)].transform;
    }

    private void Update() {
        crystalExistTimer -= Time.deltaTime;

        if( crystalExistTimer < 0 )
        {
            FinishCrystal();
        }

        if(canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                        closestTarget.position,moveSpeed * Time.deltaTime);
            
            if(Vector2.Distance(transform.position, closestTarget.position) < 1.3)
            {
                FinishCrystal();
                canMove = false;
            }
        }

        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,new Vector2(3,3),growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,cd.radius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }         
        else
            SelfDestroy();
    }

    public void SelfDestroy() =>Destroy(gameObject);
}
