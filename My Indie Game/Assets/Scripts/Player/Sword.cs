using UnityEngine;

public class Sword : MonoBehaviour
{
    Player player;
    public bool shouldCheck = true;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!shouldCheck) return;

        IDamageble damageble = other.GetComponent<IDamageble>();
        Enemy enemy = other.GetComponent<Enemy>();

        if (damageble != null && enemy.isAlive && enemy.canGetHit == true)
        {
            var damage = (float)Random.Range(player.minDamage, player.maxDamage);

            player.soundController.SwordHitSound();



            if (damage > player.maxDamage * .70f && !enemy.isDamaged)
            {
                enemy.TakeDamage(damage, true);
                enemy.ui.DisplayDamageText(damage, Color.red);
            }

            else
            {
                enemy.TakeDamage(damage, false);
                enemy.ui.DisplayDamageText(damage, Color.white);
            }
        }
    }

}
