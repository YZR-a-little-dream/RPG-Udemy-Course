using UnityEngine;
 
// 创建一个Buff效果的可序列化数据类
[CreateAssetMenu(fileName = "Buff effect",menuName = "Data/Item Effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;                  // 玩家属性
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;    // 增益效果数量
    [SerializeField] private int buffDuration;  // 增益效果持续时间

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount,buffDuration,stats.GetStat(buffType));
    }
    
  
}