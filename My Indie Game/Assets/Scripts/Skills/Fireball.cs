using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Fireball : SkillsController
{
    public override void SkillDamage(Enemy enemy)
    {
        if (enemy != null && enemy.isAlive)
        {
            var damage = (float)Random.Range(skill.minDamage, skill.maxDamage);

            var index = Random.Range(0, skill.hit_SFX.Length);
            enemy.audioSource.PlayOneShot(skill.hit_SFX[index]);

            if (damage > skill.maxDamage * skill.criticalDamageFactor && !enemy.isDamaged)
            {
                enemy.TakeDamage(damage, false);
                enemy.isOnFire = true;
            }

            else
            {
                enemy.TakeDamage(damage, false);
            }
        }
    }
}
