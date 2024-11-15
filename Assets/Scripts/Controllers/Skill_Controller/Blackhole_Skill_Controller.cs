using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;               //黑洞保持时间
    
    private bool canGrow = true;
    private bool canShrink;                      //黑洞能否缩小
    private bool canCreateHotKeys = true;
    public bool cloneAttackReleased;

    //解决在黑洞没有消失时，再按一次R会导致ReleaseCloneAttack触发从而导致玩家隐身
    private bool playerCanDisappear = true;                

    private int amountOfAttacks = 4;              //可攻击的默认值
    private float cloneAttackCooldown = 0.3f;    
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState {get; private set;}

    public void SetupBlackhole(float _maxsize, float _growSpeed, float _shrinkSpeed,
                                int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeTimer)
    {
        maxSize = _maxsize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;

        blackholeTimer = _blackholeTimer;

        if(SkillManager.instance.clone.crystalInsteadOfClone)
        {
            playerCanDisappear = false;
        }
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if(blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if(targets.Count > 0 )
            {
                ReleaseCloneAttack();
            }else
            {
                FinishBlackHoleAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp
                (transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp
                (transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if(targets.Count <= 0)
        return;

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if(playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.fx.Maketransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks >0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = UnityEngine.Random.Range(0, targets.Count);

            float xOffset;

            if (UnityEngine.Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            if(SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            }
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {   
                Invoke("FinishBlackHoleAbility",1f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotKeys()
    {
        if(createdHotKey.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(collision);
        }
    }

    //当黑洞消失后，解冻敌人
    private void OnTriggerExit2D(Collider2D collision) =>
        collision.GetComponent<Enemy>()?.FreezeTime(false);
    
    private void CreateHotKey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enough hot keys in a key code list!");
            return;
        }

        if(!canCreateHotKeys)       
            return;

        //respawn prefab of a hot key above enemy
        GameObject newHotKey = Instantiate(hotKeyPrefab,
        collision.transform.position + new Vector3(0, 2), quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[UnityEngine.Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
