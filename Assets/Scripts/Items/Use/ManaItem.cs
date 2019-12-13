using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Usable/ManaItem", fileName = "ManaItem.asset")]
public class ManaItem : UsableItem
{
    [SerializeField] int mpToRestore;
    [SerializeField] float percentToRestore;    

    public override void Action()
    {
        UpdatePlayerData();

        if (percentToRestore == 0)
        {
            player.CurMana += mpToRestore;
        }
        else
        {
            player.CurMana += Mathf.CeilToInt(player.MaxMana * percentToRestore);
        }

        if (player.CurMana > player.MaxMana)
        {
            player.CurMana = player.MaxMana;
        }

        inventory.RemoveItem(id);
    }
}
