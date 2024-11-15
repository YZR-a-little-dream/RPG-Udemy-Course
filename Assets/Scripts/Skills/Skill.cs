using Unity.Mathematics;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;                  

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update() {
        cooldownTimer -= Time.deltaTime;
    }
    
    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("Skill is coooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        //do some skill spesific things
    }

    protected virtual Transform FindClosetEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position,25);

        float closestDistance = math.INFINITY;
        Transform closestEnemy = null;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
               float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }
}