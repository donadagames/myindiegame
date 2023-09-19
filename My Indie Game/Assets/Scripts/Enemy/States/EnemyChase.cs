using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : IState
{
    private readonly EnemySpawner spawner;

    public EnemyChase(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }

    public void OnEnter()
    {
        spawner.enemy.ui.healthBar.SetActive(true);
        spawner.enemy.animator.Play(spawner.enemy.RUN);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        spawner.enemy.GetPlayerDistance(spawner.playerTarget);
        spawner.enemy.FaceTarget(spawner.playerTarget);
        spawner.enemy.MoveToTarget(spawner.playerTarget);
    }
}
