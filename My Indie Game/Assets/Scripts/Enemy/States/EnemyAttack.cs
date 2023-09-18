using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState
{
    private readonly EnemySpawner spawner;

    private float attackClipDuration;
    private float time;

    public EnemyAttack(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }

    public void OnEnter()
    {
        time = Time.time;
        spawner.canAttack = false;
        attackClipDuration = spawner.enemy.Attack();
    }

    public void OnExit()
    {
        spawner.canAttack = true;
    }

    public void Tick()
    {
        spawner.enemy.GetPlayerDistance(spawner.status.player.transform);
        spawner.enemy.FaceTarget(spawner.status.player.transform);

        if (Time.time > time + attackClipDuration)
        {
            spawner.canAttack = true;
        }
    }
}
