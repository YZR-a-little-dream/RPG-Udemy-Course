using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect",menuName = "Data/Item Effect/Thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        //setup new thunder strike
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab,
                                        _enemyPosition.position,quaternion.identity);

        Destroy(newThunderStrike,1f);
    }
}
