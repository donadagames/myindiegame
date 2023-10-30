using UnityEngine;

public class EnemyDie : IState
{
    private readonly EnemySpawner spawner;

    private float time;
    private bool shouldCheck = true;

    public EnemyDie(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }

    public void OnEnter()
    {
        time = Time.time;
        spawner.enemy.isOnFire = false;
        spawner.enemy.onFire_VFX.SetActive(false);
        spawner.enemy.ui.healthBar.SetActive(false);
        spawner.shouldSpawn = false;
        spawner.enemy.animator.Play(spawner.enemy.DIE);

        var experience = spawner.enemy.health * Random.Range(.7f, 1f);
        spawner.status.ReciveExperience(experience);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (Time.time > time + spawner.enemy.dieClipTime && shouldCheck)
        {
            shouldCheck = false;


            spawner.DeathVFX();
        }
    }
}
