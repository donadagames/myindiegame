using UnityEngine;

public class Sword : MonoBehaviour
{
    //Player player;
    Status status;
    public bool shouldCheck = true;
    public Color _color;
    private void Start()
    {
        status = Status.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!shouldCheck) return;

        IDamageble damageble = other.GetComponent<IDamageble>();
        Enemy enemy = other.GetComponent<Enemy>();

        if (damageble != null && enemy.isAlive && enemy.canGetHit == true)
        {
            var damage = (float)Random.Range(status.force, status.force * 2);

            status.player.soundController.SwordHitSound();

            if (damage > status.force * 2 * .70f && !enemy.isDamaged)
            {
                enemy.TakeDamage(damage, true);
                enemy.ui.DisplayDamageText(damage, _color);
            }

            else
            {
                enemy.TakeDamage(damage, false);
                enemy.ui.DisplayDamageText(damage, Color.white);
            }
        }
    }

}
