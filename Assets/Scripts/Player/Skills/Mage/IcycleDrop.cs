using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Mage/IcycleDrop", fileName = "IcycleDrop.asset")]
public class IcycleDrop : SingleTargetAttack
{
    public override void UseSkill()
    {
        GetTargets(); //Get our target

        if (!CheckEquippedWeapon()) //Check to see if we have the necessary weapon type equipped
        {
            Debug.Log("You can't use this skill with that weapon");
        }
        else if (targets[0] != null) //If we hit an enemy
        {
            if (UseMP())
            {
                EnemyController enemyHit = targets[0].GetComponent<EnemyController>(); //Get the enemy that we hit

                int exp = enemyHit.TakeDamage(Mathf.CeilToInt(player.GetDamage() * (damagePercent / 100.0f)), -player.transform.right);

                //If you killed it on your second hit
                if (exp != 0)
                {
                    player.Experience += exp; //Add the exp to your player
                    uiController.AddGain(exp.ToString(), "XP"); //Show on the gains UI
                }

                PlayEnemySkill();
            }
        }
    }
}
