using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
        var die = new EnemyDie(this);
        var getHit = new EnemyGetHit(this);
        var dizzy = new EnemyDizzy(this);
        var victory = new EnemyVictory(this);

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

        AddTransition(getHit, chase, EndGetHitAnimation());

        stateMachine.AddAnyTransition
            (die, () => !enemy.isAlive);
        stateMachine.AddAnyTransition
            (getHit, () => enemy.isAlive && enemy.isDamaged);
        stateMachine.AddAnyTransition
            (victory, ()=> enemy.isVictory && enemy.isAlive);

        Func<bool> IsFarAwayFromPlayer() => () => enemy.distance > chaseDistance + .5f;
        Func<bool> IsCloseToPlayerToAttack() => () => canAttack && enemy.distance < enemy.distanceToAttack;
        Func<bool> IsCloseToPlayerToChase() => () => enemy.distance < chaseDistance && enemy.distance > enemy.distanceToAttack;
        Func<bool> IsCloseToPlayerToChaseAfterAttack() => () => canAttack;
        Func<bool> EndGetHitAnimation() => () => !enemy.isDamaged && enemy.isAlive;
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

    public void DeathVFX()
    {
        Instantiate(enemy.death_VFX, enemy.transform.position + new Vector3(0,.8f,0), Quaternion.AngleAxis(-90, Vector3.left));
        if (status.enemies.Contains(enemy))
        { 
            status.enemies.Remove(enemy);
        }
        Destroy(enemy.gameObject);
    }

}
