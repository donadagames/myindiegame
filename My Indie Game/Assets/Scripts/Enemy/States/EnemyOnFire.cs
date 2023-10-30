using UnityEngine;

public class EnemyOnFire : IState
{
    private readonly EnemySpawner spawner;

    float dizzyTime;

    public EnemyOnFire(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }
    public void OnEnter()
    {
        dizzyTime = Time.time;
        spawner.enemy.animator.Play(spawner.enemy.DIZZY);
        spawner.enemy.onFire_VFX.SetActive(true);
    }
    public void OnExit()
    {
        spawner.enemy.onFire_VFX.SetActive(false);
        spawner.enemy.isDamaged = false;
    }
    public void Tick()
    {
        if (Time.time >= dizzyTime + (spawner.enemy.dizzyClipTime * 3))
        {
            spawner.enemy.isOnFire = false;
        }
    }
}
