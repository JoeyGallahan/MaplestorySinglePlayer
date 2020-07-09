using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultiTargetAttack : Skill
{
    [SerializeField][RangeAttribute(0.0f, 1000.0f)] protected float damagePercent;
    [SerializeField] protected int numTargets;
    [SerializeField] protected int targetsHit;

    protected override void GetTargets()
    {
        numTargets = targets.Length;

        for (int i = 0; i < numTargets; i++)
        {
            targets[i] = null;
        }

        UpdatePlayerData(); //Get where the player is rn. Scriptable objects are weird and dont update the variables automatically like components so we have to do it manually.

        RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, -player.transform.right, range, enemyLayer); //Get the first enemy in our path
        int ctr = 0;
        foreach(RaycastHit2D hit in hits)
        {
            if (ctr >= numTargets)
            {
                break;
            }

            if (hit.transform != null && hit.distance <= range)
            {
                targets[ctr] = hit.transform.gameObject;
            }
            else
            {
                targets[ctr] = null;
            }

            ctr++;
        }

        targetsHit = ctr;
    }

    public override abstract void UseSkill();
}
