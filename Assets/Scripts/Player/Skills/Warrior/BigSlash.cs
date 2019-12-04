using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/BigSlash", fileName = "BigSlash.asset")]
public class BigSlash : SingleTargetAttack
{
    public override void UseSkill()
    {
        GetTargets(); //Get our target

        if (targets[0] != null) //If we hit an enemy
        {
            EnemyController enemyHit = targets[0].GetComponent<EnemyController>(); //Get the enemy that we hit

            int exp = enemyHit.TakeDamage(Mathf.CeilToInt(player.GetDamage() * (damagePercent / 100.0f) ), -player.transform.right);

            if (exp != 0)
            {
                player.Experience += exp; //Add it to your player
                uiController.AddGain(exp.ToString(), "XP"); //Show on the gains UI
            }

            PlayEnemySkill();
        }
    }
}
