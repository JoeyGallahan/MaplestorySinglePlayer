using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Usable/HealingItem", fileName = "HealingItem.asset")]
public class HealingItem : UsableItem
{
    [SerializeField] int hpToHeal;
    [SerializeField] float percentToHeal;    

    public override void Action()
    {
        UpdatePlayerData();

        if (percentToHeal == 0)
        {
            player.CurHealth += hpToHeal;
        }
        else
        {
            player.CurHealth += Mathf.CeilToInt(player.MaxHealth * percentToHeal);
        }

        if (player.CurHealth > player.MaxHealth)
        {
            player.CurHealth = player.MaxHealth;
        }

        inventory.RemoveItem(id);
    }
}
