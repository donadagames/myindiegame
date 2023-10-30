using UnityEngine;

public class Fish : MonoBehaviour
{
    private int waypoint;
    public Transform[] waypoints;
    public float speed;
    private float distanceToWaypoint;

    public virtual void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x + 0.0001f, 0f, direction.z + 0.0001f));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public virtual void MoveToWaypoint()
    {
        transform.position = Vector3.MoveTowards
            (transform.position,
            waypoints[waypoint].position,
            speed * Time.deltaTime);
    }

    public void CheckIfIsCloseToWaypoint()
    {
        distanceToWaypoint = Vector3.Distance(transform.position, waypoints[waypoint].position);

        if (distanceToWaypoint < .5f)
        {
            if (waypoint < waypoints.Length - 1) { waypoint++; }
            else { waypoint = 0; }
        }
    }

    private void Update()
    {
        MoveToWaypoint();
        CheckIfIsCloseToWaypoint();
        FaceTarget(waypoints[waypoint]);
    }

}
