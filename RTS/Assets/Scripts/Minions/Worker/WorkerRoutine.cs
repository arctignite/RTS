using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerRoutine : MonoBehaviour {

    private WorkerAnimationManager anim;
    private TaskManager taskManager;
    private DeliveryTask delivery;
    private ConstructionTask construction;

    public bool hasTask, taskInitiated;
    private int taskType;
    private Task currentTask;

	// Use this for initialization
	void Start () {
        taskManager = GameObject.FindWithTag("GameManager").GetComponent<TaskManager>();
        anim = GetComponent<WorkerAnimationManager>();
        delivery = GetComponent <DeliveryTask>();
        construction = GetComponent<ConstructionTask>();
        hasTask = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!hasTask)
        {
            CheckForTask();
        }

        if (hasTask && !taskInitiated)
        {
            InitiateTask();
        }
	}

    private void CheckForTask()
    {
        //Assign task and check if it's a task
        currentTask = taskManager.AssignTask();

        if (currentTask == null)
        {
            hasTask = false;
            anim.Idle();
        }
        else
        {
            hasTask = true;
            taskType = currentTask.getTaskType();
        }
    }

    private void InitiateTask()
    {
        taskInitiated = true;

        switch (taskType)
        {
            case 0:
                //empty task
                SetTaskComplete();
                Debug.Log("something went wrong, empty task got assigned");
                break;

            case 1:
                //deliver to requester
                delivery.StartTask(currentTask, true);
                break;

            case 2:
                //deliver from requester
                delivery.StartTask(currentTask, false);
                break;

            case 3:
                //construction task
                construction.StartTask(currentTask);
                break;

            default:
                SetTaskComplete();
                Debug.Log("something went wrong, taskTyp out of range");
                break;
        }        
    }

    public void SetTaskComplete()
    {
        hasTask = false;
        taskInitiated = false;
        currentTask = null;
        anim.Idle();
    }

    public void Die()
    {
        //return task to queue
        if (currentTask != null)
        {
            taskManager.ReturnTask(currentTask);
        }

        //drop items? if so -> add task to queue to pick up items

        Destroy(gameObject);
    }
}
