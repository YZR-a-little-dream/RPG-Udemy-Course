using System;
using Unity.Mathematics;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Majar stats")]
    public Stat strength;   //力量: 1 point increase damage by 1 and crit.power by 1%
    public Stat agility;    //敏捷: 1 point increase evasion by 1% and crit chance by 1%
    public Stat intelligence;   //智力:1 point increase magic damage by 1 and magic resitance by 3
    public Stat vitality;       //活力:1 point increase health by 3 or 5 points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;      //defalut value = 150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;          //护甲
    public Stat evasion;        //闪避
    public Stat magicResistance;    //魔法抵御

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited;          // dose damage over time
    public bool isChilled;          // reduce armoe by 20%
    public bool isShocked;          // reduce accuracy by 20%

    [SerializeField] private float ailmentsDuration = 4f;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float ignitedDamageCooldown = 0.3f;    //Cooldown time of injury
    private float ignitedDamageTimer;       //Injury timer
    private int igniteDamage;

    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    public int currentHealth;

    public Action onHealthChanged;
    protected bool isDead;              //fixed The monster still receives death damage when it dies

    protected virtual void Start() {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();        //Assigning initial values

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if(isIgnited)
            ApplyIgniteDamage();
    }   

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (targetCanAvoidAttack(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();

        if(canCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            //Debug.Log("Total crit damage is " + totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        //TODO: if inventory current weapon has fire effect
        //DoMagicalDamage(_targetStats);
    }

    #region Magical damage and ailements
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private void AttemptToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (UnityEngine.Random.value < 0.3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (UnityEngine.Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("ice");
                return;
            }

            if (UnityEngine.Random.value < 0.5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("lighting");
                return;
            }
        }
        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }

        if (canApplyShock)
        {
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * 0.1f));
        }


        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        //Only one state is produced
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if(_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;
            
            fx.igniteFxFor(ailmentsDuration);
        }

        if(_chill  && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = .2f;

            GetComponent<Entity>().SlowEntityby(slowPercentage,ailmentsDuration);
            fx.chillFxFor(ailmentsDuration);
        }

        if(_shock && canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                //find closest target,only the enemies
                //instantiate thunder strike
                //setup thunder strike

                if(GetComponent<Player>() != null)
                    return;
                
                HitNearestTargetWithShockStrike();
            }
        }

    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked) 
            return;

        shockedTimer = ailmentsDuration;
        isShocked = _shock;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = math.INFINITY;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            //Find the second closest to the enemy and attack
            //Splash lightning damage
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance
                (transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            //delete if you don't want shocked target to be hit by shock strike
            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate
            (shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrike_Controller>().SetUp(shockDamage,
            closestEnemy.GetComponent<CharacterStats>());
        }
    }

    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0)
        {
            DecreaseHealthyBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    #endregion

    //收到伤害
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthyBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFx");

        if(currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    //Reducing health alone has no other effect
    protected virtual void DecreaseHealthyBy(int _damage)
    {
        currentHealth -= _damage;

        onHealthChanged?.Invoke();
    }

    protected virtual void Die()
    {
        isDead = true;
    }
    
    #region Stat Calculations
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (3 * _targetStats.intelligence.GetValue());
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    private bool targetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.agility.GetValue() + _targetStats.evasion.GetValue();

        if(isShocked)
        {
            totalEvasion += 20;             //眩晕后增加闪避20
        }
        
        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        
        return false;
    }

    private bool canCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(UnityEngine.Random.Range(0,100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        //Debug.Log("total crit power %" + totalCritPower);

        float critDamage = _damage  * totalCritPower;
        //Debug.Log("crit damage before round up" + critDamage);
        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;

    #endregion
}
