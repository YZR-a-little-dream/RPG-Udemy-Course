using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_skill : Skill
{
    [Header("Clone Timer")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneCounterAttacck;
    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;
    
    
    public void CreateClone(Transform _clonePosition,Vector3 _offset = new Vector3())
    {
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            //SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition,
            cloneDuration,canAttack,_offset,FindClosetEnemy(newClone.transform),
            canDuplicateClone,chanceToDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if(createCloneOnDashStart)
        {
            CreateClone(player.transform);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if(createCloneOnDashOver)
        {
            CreateClone(player.transform);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if(canCreateCloneCounterAttacck)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform,
                new Vector3(2 * player.facingDir,0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform,Vector3 _offset = new Vector3())
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform,_offset);
    }
}
