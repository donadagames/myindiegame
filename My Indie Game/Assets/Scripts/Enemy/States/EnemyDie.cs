using System.Collections;
using System.Collections.Generic;
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
        spawner.enemy.ui.healthBar.SetActive(false);
        spawner.shouldSpawn = false;
        spawner.enemy.animator.Play(spawner.enemy.DIE);
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
