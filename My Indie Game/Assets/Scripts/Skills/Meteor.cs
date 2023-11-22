using UnityEngine;

public class Meteor : SkillsController
{
    public Color _color;

    public override void SkillDamage(Enemy enemy)
    {
        if (enemy != null && enemy.isAlive && enemy.canGetHit)
        { 
            float damage = (float)Random.Range(skill.minDamage, skill.maxDamage);

            var index = Random.Range(0, skill.hit_SFX.Length);
            enemy.audioSource.PlayOneShot(skill.hit_SFX[index]);
            enemy.TakeDamage(damage, false);
            enemy.isOnFire = true;
            enemy.ui.DisplayDamageText(damage, _color);
        }
    }
}
