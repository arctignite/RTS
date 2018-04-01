using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathFinding : MonoBehaviour {
    public Transform targetPosition;

    private Seeker seeker;
    private CharacterController controller;
    private WorkerStats stats;

    public Path path;

    private float nextWayPointDistance = 1;
    private int currentWaypoint = 0;

    private float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    private bool atDestination;

	void Start () {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        stats = GetComponent<WorkerStats>();
        atDestination = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (atDestination) //stops all movement once destination is reached
        {
            return;
        }
        else
        {
            if (Time.time > lastRepath + repathRate && seeker.IsDone())
            {
                lastRepath = Time.time;

                //Start a new path to the targetPosition, call the OnPathComplete function
                //when the path has been calculated (could take a few frames)
                seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            }

            if (path == null)
            {
                //no active path yet, so do nothing
                return;
            }

            if (currentWaypoint > path.vectorPath.Count) return;

            if (currentWaypoint == path.vectorPath.Count)
            {
                OnDestinationReached();
                currentWaypoint++;
                return;
            }

            //Direction to the next waypoint, normalized so it has the length of 1 world unit
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

            //multiply with speed to get velocity
            Vector3 velocity = dir * stats.GetMovementSpeed();

            //SimpleMove takes velocity in m/s, so no need to multiply by time.DeltaTime
            controller.SimpleMove(velocity);

            if (dir != Vector3.zero) //rotates the model towards the direction of movement
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * stats.GetMovementSpeed()
                );
            }

            if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWayPointDistance * nextWayPointDistance)
            {
                currentWaypoint++;
                return;
            }
        }
        
	}

    void OnPathComplete(Path p)
    {        
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0; //reset the waypoint counter so we start at the beginning of the path
        }
        else
        {
            Debug.Log("error encountered with pathfinding: " + p.error);
        }
    }

    void OnDestinationReached()
    {
        atDestination = true;
    }

    public void MoveToDestination(Transform _destination)
    {
        targetPosition = _destination;
        atDestination = false;
    }

    public bool GetAtDestination()
    {
        return atDestination;
    }

    public void OnDisable()
    {
        seeker.pathCallback -= OnPathComplete;
    }
}
