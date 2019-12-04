using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/DoubleSlash", fileName = "DoubleSlash.asset")]
public class DoubleSlash : SingleTargetAttack
{
    public override void UseSkill()
    {
        GetTargets(); //Get our target

        if (targets[0] != null) //If we hit an enemy
        {
            EnemyController enemyHit = targets[0].GetComponent<EnemyController>(); //Get the enemy that we hit

            //Double slash hits twice and we want the damage popup to go twice so just call the method again
            int exp = enemyHit.TakeDamage(Mathf.CeilToInt(player.GetDamage() * (damagePercent / 100.0f) ), -player.transform.right);
            int exp2 = enemyHit.TakeDamage(Mathf.CeilToInt(player.GetDamage() * (damagePercent / 100.0f) ), -player.transform.right);

            //If you killed it on your second hit
            if (exp2 != 0 && exp2 >= exp)
            {
                player.Experience += exp2; //Add the exp to your player
                uiController.AddGain(exp2.ToString(), "XP"); //Show on the gains UI
            }
            else if (exp != 0)
            {
                player.Experience += exp; //Add it to your player
                uiController.AddGain(exp2.ToString(), "XP"); //Show on the gains UI
            }

            PlayEnemySkill();
        }
    }
}
