using System.Collections;
using System.Collections.Generic;
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
        spawner.enemy.animator.Play(spawner.enemy.GETHIT);
    }
    public void OnExit()
    {
        spawner.enemy.hasTakenCritialDamage = false;
    }

    public void Tick()
    {
        if (Time.time > time + spawner.enemy.getHitClipTime)
        {
            spawner.enemy.hasTakenCritialDamage = false;
        }
    }
}
