using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : IState
{
    private readonly EnemySpawner spawner;

    public EnemyIdle(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }

    public void OnEnter()
    {
        spawner.enemy.animator.Play(spawner.enemy.IDLE);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        spawner.enemy.GetPlayerDistance(spawner.status.player.transform);
        spawner.enemy.FaceTarget(spawner.status.player.transform);
    }
}
