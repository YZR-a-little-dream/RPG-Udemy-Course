using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;

    private Animator anim;
    private bool triggered;

    private void Start() {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (!targetStats ||  triggered)  
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position,
        targetStats.transform.position,speed * Time.deltaTime);

        transform.right = transform.position - targetStats.transform.position;

        if(Vector2.Distance(transform.position,targetStats.transform.position) < 0.1f)
        {
            //Avoid lightning hitting the ground
            anim.transform.localPosition = new Vector3(0,0.5f);
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3,3);

            Invoke("DamageAndSelfDestroy",.2f);
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }

    public void SetUp(int _damage,CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject,0.4f);
    }
}
