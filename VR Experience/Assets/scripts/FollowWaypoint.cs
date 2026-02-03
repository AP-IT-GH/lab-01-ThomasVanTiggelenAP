using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    public Transform[] waypoints;
    public int currentWaypoint = 0;

    public float speed;
    public float rotSpeed;
    public float distanceThreshold;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 direction = waypoints[currentWaypoint].transform.position - transform.position; Quaternion lookRotation = Quaternion.LookRotation(direction); transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
}
