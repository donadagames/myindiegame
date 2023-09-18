using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Enemy enemy;
    public bool shouldSpawn = true;
    public Status status;
    public float chaseDistance = 6;
    public StateMachine stateMachine;
    public bool canAttack = true;
    private EnemyIdle idle;
    private EnemyChase chase;

    public EnemyAttackPosition enemyPosition;
    public Transform playerTarget;

    private void Awake()
    {
        stateMachine = new StateMachine();

        idle = new EnemyIdle(this);
        chase = new EnemyChase(this);
        var attack = new EnemyAttack(this);
        var patroll = new EnemyPatroll(status, this);
        var die = new EnemyDie(status, this);
        var getHit = new EnemyGetHit(status, this);
        var dizzy = new EnemyDizzy(status, this);
        var victory = new EnemyVictory(status, this);

        void AddTransition(IState from, IState to,
            Func<bool> condition) =>
            stateMachine.AddTransition
            ((IState)from, (IState)to, condition);

        AddTransition
            (idle, chase, IsCloseToPlayerToChase());

        AddTransition
            (chase, attack, IsCloseToPlayerToAttack());
        AddTransition
           (chase, idle, IsFarAwayFromPlayer());


        AddTransition
            (attack, chase, IsCloseToPlayerToChaseAfterAttack());
        // AddTransition
        //(attack, idle, IsCloseToPlayerToIdleAfterAttack());
        //AddTransition
        // (attack, attack, IsCloseToPlayerToAttackAfterAttack());


        Func<bool> IsFarAwayFromPlayer() => () => enemy.distance > chaseDistance;
        Func<bool> IsCloseToPlayerToAttack() => () => canAttack && enemy.distance < enemy.distanceToAttack;
        Func<bool> IsCloseToPlayerToChase() => () => enemy.distance < chaseDistance && enemy.distance > enemy.distanceToAttack;
        Func<bool> IsCloseToPlayerToChaseAfterAttack() => () => canAttack; //&& enemy.distance < chaseDistance && enemy.distance > enemy.distanceToAttack;
        //Func<bool> IsCloseToPlayerToAttackAfterAttack() => () => canAttack && enemy.distance < enemy.distanceToAttack;
        Func<bool> IsCloseToPlayerToIdleAfterAttack() => () => canAttack && enemy.distance > chaseDistance;
    }

    private void Start()
    {
        status = Status.instance;
    }

    public void SpawEnemy()
    {
        if (shouldSpawn && enemy == null)
        {
            shouldSpawn = false;
            var _enemy =
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy = _enemy.GetComponent<Enemy>();
            enemy.spawner = this;
            enemyPosition = status.player.GetEnemyPosition();

            if (enemyPosition == null)
            {
                playerTarget = status.player.transform;
            }
            else
            {
                playerTarget = enemyPosition.transform;
            }

            stateMachine.SetState(idle);
        }

        else return;
    }

    public void ResetEnemyPositionIndex()
    {
        enemyPosition.isInUse = false;
    }

}
