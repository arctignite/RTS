using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTask : MonoBehaviour {

    private WorkerRoutine routine;
    private WorkerStats stats;
    private WorkerAnimationManager anim;
    private PathFinding path;
    private Task task;

    private int currentTaskStep;
    private bool taskInitiated;
    private bool constructionInProgress;

    private Transform destination;

	// Use this for initialization
	void Start () {
        routine = GetComponent<WorkerRoutine>();
        stats = GetComponent<WorkerStats>();
        anim = GetComponent<WorkerAnimationManager>();
        path = GetComponent<PathFinding>();
        taskInitiated = false;
        constructionInProgress = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (taskInitiated && !constructionInProgress)
        {
            switch (currentTaskStep)
            {
                case 0:
                    MoveToLocation();
                    break;

                case 1:
                    if (path.GetAtDestination())
                    {
                        ArrivedAtDestination();
                    }
                    break;

                case 2:
                    StartConstruction();
                    break;

                default:
                    Debug.Log("Error: construction taskStep out of range");
                    break;
            }

        }
    }

    public void StartTask(Task _task)
    {
        task = _task;
        anim.EquipHammer();
        destination = task.getRequester();
        taskInitiated = true; //initate the task
        constructionInProgress = false;
        currentTaskStep = 0;
    }

    private void MoveToLocation() //step 0
    {
        path.MoveToDestination(destination);
        anim.Walking();
        currentTaskStep++;
    }

    private void ArrivedAtDestination() //step 1
    {
        anim.Idle();
        currentTaskStep++;
    }

    private void StartConstruction() //step 2
    {
        anim.Attacking();
        constructionInProgress = true;
        task.getRequester().GetComponent<Construction>().AddBuilder(gameObject);
    }

    public void StopConstruction()
    {
        //call from construction whenever out of resources or finished
        constructionInProgress = false;
        anim.Idle();
        taskInitiated = false;

        try
        {
            task.getRequester().GetComponent<Construction>().RemoveBuilder(gameObject);
        }
        catch
        {
            Debug.Log("couldn't remove builder from construction");
        }

        currentTaskStep++;
        routine.SetTaskComplete();
    }
}
