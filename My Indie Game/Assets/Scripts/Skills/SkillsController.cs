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
            enemy.TakeDamage(damage, true);
        }
    }

}

public interface ISkillDamage
{
    void SkillDamage(Enemy enemy);
}