using Unity.VisualScripting;
using UnityEngine;

public class SkillsController : MonoBehaviour, ISkillDamage
{
    public Skill skill;

  

    public virtual void SkillDamage(Enemy enemy)
    {
        if (enemy != null && enemy.isAlive)
        {
            var damage = (float)Random.Range(skill.minDamage, skill.maxDamage);

            var index = Random.Range(0, skill.hit_SFX.Length);
            enemy.audioSource.PlayOneShot(skill.hit_SFX[index]);

            if (damage > skill.maxDamage * skill.criticalDamageFactor && !enemy.isDamaged)
            {
                enemy.TakeDamage(damage, true);
            }

            else
            {
                enemy.TakeDamage(damage, false);
            }
        }
    }

}

public interface ISkillDamage
{
    void SkillDamage(Enemy enemy);
}