using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleTargetAttack : Skill
{
    [SerializeField][RangeAttribute(0.0f, 1000.0f)] protected float damagePercent;

    protected override void GetTargets()
    {
        UpdatePlayerData(); //Get where the player is rn. Scriptable objects are weird and dont update the variables automatically like components so we have to do it manually.

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, -player.transform.right, range, enemyLayer); //Find an enemy in our path

        if (hit.transform != null)
        {
            targets[0] = hit.transform.gameObject;
        }
        else
        {
            targets[0] = null;
        }
    }

    public override abstract void UseSkill();
}
