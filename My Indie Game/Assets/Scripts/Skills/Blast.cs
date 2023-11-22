using UnityEngine;

public class Blast : SkillsController
{
    public Color _color;
    public override void SkillDamage(Enemy enemy)
    {
        if (enemy != null && enemy.isAlive)
        {
            var damage = (float)Random.Range(skill.minDamage, skill.maxDamage);

            var index = Random.Range(0, skill.hit_SFX.Length);
            enemy.audioSource.PlayOneShot(skill.hit_SFX[index]);
            enemy.TakeDamage(damage, false);
            enemy.isDizzy = true;
            enemy.ui.DisplayDamageText(damage, _color);
            Instantiate(skill.enemy_Hit_VFX, enemy.transform.position + new Vector3(0, .5f, 0), Quaternion.identity, enemy.transform);
            enemy.rb.AddForce(new Vector3(0, enemy.rb.mass * Random.Range(42, 52), 0), ForceMode.Acceleration);
        }
    }
}
