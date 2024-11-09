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
    [SerializeField] private float useTimeWindow; //在使用水晶后长时间不使用将填充水晶
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
                
                //当我们使用第一个水晶时
                if(crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility",useTimeWindow);
                }
                cooldown = 0;           //当填充水晶后重置冷却时间
                
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];//选择最后一颗水晶
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
        //保持水晶数永远为amountOfStacks
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
