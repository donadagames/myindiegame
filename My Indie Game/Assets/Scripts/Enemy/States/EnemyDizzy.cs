using UnityEngine;

public class EnemyDizzy : IState
{
    private readonly EnemySpawner spawner;

    float dizzyTime;

    public EnemyDizzy(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }
    public void OnEnter()
    {
        dizzyTime = Time.time;
        spawner.enemy.animator.Play(spawner.enemy.DIZZY);
        spawner.enemy.dizzy_VFX.SetActive(true);
    }
    public void OnExit()
    {
        spawner.enemy.dizzy_VFX.SetActive(false);
        spawner.enemy.isDamaged = false;
        spawner.enemy.isOnFire = false;
        spawner.enemy.isDizzy = false;
        spawner.enemy.isFreezed = false;
        spawner.enemy.shouldCheckParticleHit = true;
    }
    public void Tick()
    {
        if (Time.time >= dizzyTime + (spawner.enemy.dizzyClipTime * 5))
        {
            spawner.enemy.isDizzy = false;
        }
    }
}
