using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect",menuName = "Data/Item Effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    
    [Range(0,1f)]                                       
    [SerializeField] private float healPercent;         

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        //player stats
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        //how much to heal (Percentage treatment)
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        //real heal
        playerStats.IncreaseHelathBy(healAmount);
    }

}
