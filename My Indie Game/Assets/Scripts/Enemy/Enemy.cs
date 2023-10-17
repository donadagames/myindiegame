using UnityEngine;

public class Enemy : MonoBehaviour, IDamageble
{
    public EnemySpawner spawner;
    public float distance;
    public float distanceToWaypoint;

    public float scapeDistance = 11;
    //public Transform target;
    public Animator animator;
    public float distanceToAttack;
    public float chasingVelocity;

    public GameObject death_VFX;

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

    public float patrollVelocity;
    public int waypoint;

    public bool isAlive = true;

    public float health;
    public float currentHealth;
    public int minDamage;
    public int maxDamage;
    public EnemyUI ui;
    public bool isDamaged = false;
    public bool isVictory = false;

    public virtual void Update()
    {
        spawner.stateMachine.Tick();
    }

    public virtual void Start()
    {
        currentHealth = health;
        ui.SetMaxHealth(currentHealth);
        ui.healthBar.SetActive(false);
    }

    public virtual void GetPlayerDistance(Transform target)
    {
        distance = Vector3.Distance(transform.position, target.position);

        if (distance > scapeDistance)
        {
            spawner.shouldSpawn = true;
            Destroy(this.gameObject);


            if (spawner.enemyPosition != null)
                spawner.enemyPosition.isInUse = false;
        }
    }

    public void CheckScapeDistance()
    {

    }

    public virtual void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x + 0.0001f, 0f, direction.z + 0.0001f));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public virtual void MoveToTarget(Transform target)
    {
        transform.position =
            Vector3.MoveTowards
            (transform.position, target.position, chasingVelocity * Time.deltaTime);
    }

    public virtual void MoveToWaypoint()
    {
        transform.position = Vector3.MoveTowards
            (transform.position,
            spawner.waypoints[waypoint].position,
            patrollVelocity * Time.deltaTime);
    }

    public void CheckIfIsCloseToWaypoint()
    {
        distanceToWaypoint = Vector3.Distance(transform.position, spawner.waypoints[waypoint].position);

        if (distanceToWaypoint < spawner.minDistanceToWaypoint)
        {
            if (waypoint < spawner.waypoints.Length - 1) { waypoint++; }
            else { waypoint = 0; }
        }
    }

    public float Attack()
    {
        var index = UnityEngine.Random.Range(0, ATTACKS.Length);
        animator.Play(ATTACKS[index]);

        return attackClipTime[index];
    }

    public virtual void TakeDamage(float damage, bool isCritical)
    {
        if (isAlive)
        {
            currentHealth -= damage;
            ui.SetValue(currentHealth);
            CheckIfIsDead();
            isDamaged = isCritical;
        }
    }

    public virtual void CheckIfIsDead()
    {
        if (currentHealth <= 0)
        {
            isAlive = false;
        }
    }

    public void Damage()
    {
        var damage = UnityEngine.Random.Range(minDamage, maxDamage);
        var isCritical = damage >= maxDamage * .8f;
        spawner.status.TakeDamage(damage, isCritical);
    }

}

public interface IDamageble
{
    public void TakeDamage(float damage, bool isCritical);
}