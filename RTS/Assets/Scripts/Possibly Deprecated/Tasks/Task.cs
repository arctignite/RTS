using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Task 
{

    /* taskType description   
        taskType int:
            0: empty task
            1: deliver to requester
            2: deliver from requester
            3: construction
            4: quarry
            5: woodcutter
    */
 
    public float priority;   
    public float age;

    public Transform requester;
    public int taskType;
    public ResourceList resources;

    //Constructor
    public Task(ResourceList _resources, float _priority, float _age, Transform _requester, int _taskType)
    {
        resources = _resources;
        priority = _priority;
        requester = _requester;
        age = _age;
        taskType = _taskType;
    }

    public ResourceList getResources()
    {
        return resources;
    }

    public Transform getRequester()
    {
        return requester;
    }

    public float getPriority()
    {
        return priority;
    }

    public float getAge()
    {
        return age;
    }

    public int getTaskType()
    {
        return taskType;
    }
}
