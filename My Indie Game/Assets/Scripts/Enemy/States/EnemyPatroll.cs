using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroll : IState
{
    // Start is called before the first frame update
    private readonly EnemySpawner spawner;

    public EnemyPatroll(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }
    public void OnEnter()
    {
        spawner.enemy.ui.healthBar.SetActive(false);
        spawner.enemy.animator.Play(spawner.enemy.WALK);
    }
    public void OnExit()
    {
    }
    public void Tick()
    {
        spawner.enemy.CheckIfIsCloseToWaypoint();
        spawner.enemy.GetPlayerDistance(spawner.playerTarget);
        spawner.enemy.FaceTarget(spawner.waypoints[spawner.enemy.waypoint]);
        spawner.enemy.MoveToWaypoint();
    }
}
