using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow; //��ʹ��ˮ����ʱ�䲻ʹ�ý����ˮ��
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if(CanUseMultiCrystal())
            return;

        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if(canMoveToEnemy)
            {
                return;
            }

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
            
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        
        currentCrystalScript.SetupCryStal(crystalDuration,
        canExplode, canMoveToEnemy, moveSpeed, FindClosetEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget() => 
        currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if(canUseMultiStacks)
        {
            //respawn crystal
            if(crystalLeft.Count > 0 )
            {
                
                //������ʹ�õ�һ��ˮ��ʱ
                if(crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility",useTimeWindow);
                }
                cooldown = 0;           //�����ˮ����������ȴʱ��
                
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];//ѡ�����һ��ˮ��
                GameObject newCrystal = Instantiate
                    (crystalToSpawn,player.transform.position,Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCryStal(
                    crystalDuration,canExplode,canMoveToEnemy,moveSpeed,FindClosetEnemy(newCrystal.transform)
                );

                if(crystalLeft.Count <= 0)
                {
                    //cooldown the skill 
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                    //refill crystal
                }           
                return true;
            }
        }

        return false;
    }

    private void RefilCrystal()
    {
        //����ˮ������ԶΪamountOfStacks
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if(cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }

}
