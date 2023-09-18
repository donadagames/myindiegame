using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    public EnemySpawner spawner;
    public float distance;
    public float scapeDistance = 11;
    public Transform target;
    public Animator animator;
    public float distanceToAttack;
    public float chasingVelocity;
    public string IDLE;
    public string WALK;
    public string RUN;
    public string GETHIT;
    public string DIE;
    public string DIZZY;
    public string VICTORY;
    public string[] ATTACKS;
    public float[] attackClipTime;
    public float getHitClipTime;
    public float dieClipTime;
    public float dizzyClipTime;

    private void Update()
    {
        spawner.stateMachine.Tick();
    }

    public void GetPlayerDistance(Transform target)
    {
        distance = Vector3.Distance(transform.position, target.position);

        if (distance > scapeDistance)
        {
            spawner.shouldSpawn = true;
            spawner.enemyPosition.isInUse = false;
            Destroy(this.gameObject);
        }
    }

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x + 0.0001f, 0f, direction.z + 0.0001f));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void MoveToTarget(Transform target)
    {
        transform.position =
            Vector3.MoveTowards
            (transform.position, target.position, chasingVelocity * Time.deltaTime);
    }

    public float Attack()
    {
        var index = UnityEngine.Random.Range(0, ATTACKS.Length);
        animator.Play(ATTACKS[index]);

        return attackClipTime[index];
    }
}
