using UnityEngine;

public class Clone_skill : MonoBehaviour
{
    [Header("Clone Timer")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;
    
    public void CreateClone(Transform _clonePosition,Vector3 _offset = new Vector3())
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition,cloneDuration,canAttack,_offset);

    }
}
