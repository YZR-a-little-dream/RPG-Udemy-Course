using UnityEngine;
 
// ����һ��BuffЧ���Ŀ����л�������
[CreateAssetMenu(fileName = "Buff effect",menuName = "Data/Item Effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;                  // �������
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;    // ����Ч������
    [SerializeField] private int buffDuration;  // ����Ч������ʱ��

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount,buffDuration,stats.GetStat(buffType));
    }
    
  
}