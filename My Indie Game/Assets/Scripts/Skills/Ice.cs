using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : SkillsController
{
    public override void SkillDamage(Enemy enemy)
    {
        if (enemy != null && enemy.isAlive)
        {
            var damage = (float)Random.Range(skill.minDamage, skill.maxDamage);

            var index = Random.Range(0, skill.hit_SFX.Length);
            enemy.audioSource.PlayOneShot(skill.hit_SFX[index]);
            enemy.TakeDamage(damage, false);
            enemy.isFreezed = true;
            enemy.ui.DisplayDamageText(damage, Color.blue);
        }
    }
}