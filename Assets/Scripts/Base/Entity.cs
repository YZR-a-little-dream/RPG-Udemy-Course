using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    
    #endregion

    #region  Collidion info
    [Header("Collision info")]                                                          
    [SerializeField] protected Transform groundCheck;             //地面检查
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;               //墙壁检查
    [SerializeField] protected float wallCheckDistance;   
    [SerializeField] protected LayerMask whatIsGround;            //准备射线检测的layerMask
    #endregion

    public int facingDir {get; private set;} = 1;               //玩家面朝的方向
    private bool facingRight = true;


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }
    
    #region  Velocity
    public void SetZeroVelocity() => rb.velocity = Vector2.zero;

    public void SetVelocity(float _xVelocity,float _yVelocity)
    {
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
