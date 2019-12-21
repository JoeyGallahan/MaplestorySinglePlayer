using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Mage/PurifyingFlame", fileName = "PurifyingFlame.asset")]
public class PurifyingFlames : MultiTargetAttack
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
                for (int i = 0; i < targetsHit; i++) //Go through each enemy we hit
                {
                    EnemyController enemyHit = targets[i].GetComponent<EnemyController>(); //Get an enemy

                    int exp = enemyHit.TakeDamage(Mathf.CeilToInt(player.GetDamage() * (damagePercent / 100.0f)), -player.transform.right);

                    if (exp != 0)
                    {
                        player.Experience += exp; //Add it to your player
                        uiController.AddGain(exp.ToString(), "XP"); //Show on the gains UI
                    }

                    PlayEnemySkill();
                }
            }
        }
    }
}
