using UnityEngine;

public class EnemyGetHit : IState
{
    // Start is called before the first frame update
    private readonly EnemySpawner spawner;
    public float time;

    public EnemyGetHit(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }
    public void OnEnter()
    {
        time = Time.time;   
        spawner.enemy.isOnFire = false;
        spawner.enemy.animator.Play(spawner.enemy.GETHIT);
    }
    public void OnExit()
    {
        spawner.enemy.isDamaged = false;
        spawner.enemy.isOnFire = false;
        spawner.enemy.isFreezed = false;
        spawner.enemy.shouldCheckParticleHit = true;
        spawner.enemy.onFire_VFX.SetActive(false);
    }

    public void Tick()
    {
        if (Time.time > time + spawner.enemy.getHitClipTime)
        {
            spawner.enemy.isDamaged = false;
        }
    }
}
