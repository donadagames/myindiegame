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
        spawner.enemy.isFreezed = false;
        spawner.enemy.isDizzy = false;
        spawner.enemy.onFire_VFX.SetActive(false);
        spawner.enemy.dizzy_VFX.SetActive(false);
        spawner.enemy.freezed_VFX.SetActive(false);
        spawner.enemy.ui.gameObject.SetActive(false);
        spawner.shouldSpawn = false;
        spawner.enemy.animator.Play(spawner.enemy.DIE);
        spawner.enemy.shouldCheckParticleHit = false;
        var experience = spawner.enemy.experience;
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
