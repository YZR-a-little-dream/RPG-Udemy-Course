using System;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private String statName;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statNameTxet;
    [SerializeField] private TextMeshProUGUI statValueText;
    
    private void OnValidate() {
        gameObject.name = "Stat - " + statName;

        if(statNameTxet != null)
            statNameTxet.text = statName;
    }

    private void Start() {
        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
        }
    }
}
