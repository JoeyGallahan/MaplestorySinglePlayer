using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/DoubleSlash", fileName = "DoubleSlash.asset")]
public class DoubleSlash : SingleTargetAttack
{
    public override void UseSkill()
    {
        GetTargets(); //Get our target

        if (targets[0] == null) //If we didnt hit anything just do a little debug. Should remove in the future.
        {
            Debug.Log("Skill Used: " + skillName + ". Hit: Nobody");
        }
        else //If we hit an enemy
        {
            Debug.Log("Skill Used: " + skillName + ". Hit: " + targets[0].name);
            EnemyController enemyHit = targets[0].GetComponent<EnemyController>(); //Get the enemy that we hit

            //Double slash hits twice and we want the damage popup to go twice so just call the method again
            int exp = enemyHit.TakeDamage(Mathf.FloorToInt(player.GetDamage() * (damagePercent / 100.0f) ), -player.transform.right);
            int exp2 = enemyHit.TakeDamage(Mathf.FloorToInt(player.GetDamage() * (damagePercent / 100.0f) ), -player.transform.right);

            //If you killed it on your second hit
            if (exp2 != 0 && exp2 >= exp)
            {
                player.Experience += exp2; //Add the exp to your player
            }
            else if (exp != 0)
            {
                player.Experience += exp; //Add it to your player
            }

            PlayEnemySkill();
        }
    }
}
