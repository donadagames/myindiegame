using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreezed : IState
{
    private readonly EnemySpawner spawner;

    float dizzyTime;

    public EnemyFreezed(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }
    public void OnEnter()
    {
        dizzyTime = Time.time;
        spawner.enemy.animator.Play(spawner.enemy.DIZZY);

        if (spawner.enemy.freezed_VFX.activeSelf == true)
            spawner.enemy.freezed_VFX.SetActive(false);

        spawner.enemy.freezed_VFX.SetActive(true);
    }
    public void OnExit()
    {
        spawner.enemy.isDamaged = false;
        spawner.enemy.isOnFire = false;
        spawner.enemy.isFreezed = false;
        spawner.enemy.shouldCheckParticleHit = true;
    }
    public void Tick()
    {
        if (Time.time >= dizzyTime + (spawner.enemy.dizzyClipTime * 5))
        {
            spawner.enemy.isFreezed = false;
        }
    }
}
