using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVictory : IState
{
    // Start is called before the first frame update
    private readonly EnemySpawner spawner;

    public EnemyVictory(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }
    public void OnEnter()
    {
        spawner.enemy.animator.Play(spawner.enemy.VICTORY);
    }
    public void OnExit()
    {
    }
    public void Tick()
    {
    }
}
