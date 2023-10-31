using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Fireball : SkillsController
{
    public override void SkillDamage(Enemy enemy)
    {
        if (enemy != null && enemy.isAlive && enemy.canGetHit)
        {
            var damage = (float)Random.Range(skill.minDamage, skill.maxDamage);

            var index = Random.Range(0, skill.hit_SFX.Length);
            enemy.audioSource.PlayOneShot(skill.hit_SFX[index]);
            enemy.TakeDamage(damage, false);
            enemy.isOnFire = true;
            enemy.ui.DisplayDamageText(damage, Color.red);
        }
    }
}
