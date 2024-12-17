using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect",menuName = "Data/Item Effect/Freeze enemies effect")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        //只有小于最大生命值的百分之10时，才触发冻结技能
        //The freeze ability is triggered only when it is less than 10% of the maximum health
        if(playerStats.currentHealth > 0.1f * playerStats.GetMaxHealthValue())
            return;

        if(!Inventory.instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position,2);

        foreach(var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
            
        }
    }
}
