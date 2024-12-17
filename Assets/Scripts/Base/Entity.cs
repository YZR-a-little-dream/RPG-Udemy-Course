using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr{ get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDiretion;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    #region  Collidion info
    [Header("Collision info")]
    public Transform attackCheck;                  
    public float attackCheckRadius;                 //攻击检测半径                                                          
    [SerializeField] protected Transform groundCheck;             //地面检查
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;               //墙壁检查
    [SerializeField] protected float wallCheckDistance;   
    [SerializeField] protected LayerMask whatIsGround;            //准备射线检测的layerMask
    #endregion

    public int facingDir {get; private set;} = 1;               //玩家面朝的方向
    private bool facingRight = true;

    public Action onFlipped;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    public virtual void SlowEntityby(float _slowPercentage,float _slowDuration)
    {

    }

    protected virtual void RetuenDefaultSpeed()
    {
        anim.speed = 1;
    }


    protected virtual void Update()
    {

    }
    
    public virtual void DamageImpact() => StartCoroutine("HitKnockback");
        
    

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockbackDiretion.x * -facingDir,knockbackDiretion.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region  Velocity
    public void SetZeroVelocity() {
        if(isKnocked) return;           //避免在攻击时候，无法被击退
        rb.velocity = Vector2.zero;
    } 

    public void SetVelocity(float _xVelocity,float _yVelocity)
    {
        if(isKnocked)
           return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FilpController(_xVelocity);
    }
    #endregion

    #region Collision  
    public virtual bool IsGroundedDetected() => 
        Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);

    public virtual bool IsWallDeteced() =>
        Physics2D.Raycast(wallCheck.position,Vector2.right * facingDir,wallCheckDistance,whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x 
            + wallCheckDistance,wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);
    }
    #endregion

    #region  Filp
    public virtual void Filp()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        onFlipped?.Invoke();
    }

    public virtual void FilpController(float _x)
    {
        if(_x > 0 && !facingRight)
        {
            Filp();
        }else if(_x < 0 && facingRight)
        {
            Filp();
        }
    }
    #endregion

    public virtual void Die()
    {
        
    }
}
