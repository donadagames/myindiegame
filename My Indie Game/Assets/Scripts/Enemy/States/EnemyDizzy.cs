using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDizzy : IState
{
    // Start is called before the first frame update
    private readonly EnemySpawner spawner; private readonly Status status;

    public EnemyDizzy(Status _status, EnemySpawner _spawner)
    {
        status = _status;
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
    }
}
