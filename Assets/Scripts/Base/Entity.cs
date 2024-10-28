using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fX { get; private set; }
    
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDiretion;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    #region  Collidion info
    [Header("Collision info")]
    public Transform attackCheck;                  
    public float attackCheckRadius;                 //�������뾶                                                          
    [SerializeField] protected Transform groundCheck;             //������
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;               //ǽ�ڼ��
    [SerializeField] protected float wallCheckDistance;   
    [SerializeField] protected LayerMask whatIsGround;            //׼�����߼���layerMask
    #endregion

    public int facingDir {get; private set;} = 1;               //����泯�ķ���
    private bool facingRight = true;


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        fX = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }
    
    public virtual void Damage()
    {
        fX.StartCoroutine("FlashFx");
        StartCoroutine("HitKnockback");
        //Debug.Log(gameObject.name + " was damaged!");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        
        rb.velocity = new Vector2(knockbackDiretion.x * -facingDir,knockbackDiretion.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region  Velocity
    public void SetZeroVelocity() {
        if(isKnocked) return;           //�����ڹ���ʱ���޷�������
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

}
