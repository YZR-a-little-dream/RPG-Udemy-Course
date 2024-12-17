using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthyBy(int _damage)
    {
        base.DecreaseHealthyBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        currentArmor?.Effect(player.transform);
    }
}
