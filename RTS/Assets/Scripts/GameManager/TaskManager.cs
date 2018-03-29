using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour {

    public List<Task> taskList = new List<Task>();
    private Task nullTask = new Task(null, 0, 0, null, 0);
    float age, priority, x;
    Task task;

    public void NewTask(ResourceList _resources, float _priority, float _age, Transform _requester, int _taskType)
    {
        taskList.Add(new Task(_resources, _priority, _age, _requester, _taskType));
    }

    public void ReturnTask(Task t)
    {
        //used to give task back in case of failure
        taskList.Add(t);
    }

    //assign task to a worker
    public Task AssignTask()
    {
        if (taskList.Count == 0)
        {
            return null;
        }
        else
        {
            task = nullTask;
            x = 0;

            foreach (Task i in taskList)
            {
                age = Time.time - i.getAge();
                priority = age * i.getPriority();

                if (priority > x)
                {
                    x = priority;
                    task = i;
                }
            }

            taskList.Remove(task);
            return task;
        }       
    }
}
