using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTask : MonoBehaviour {

    private WorkerRoutine routine;
    private TotalInventory inventory;
    private WorkerStats stats;
    private WorkerAnimationManager anim;
    private PathFinding path;

    private ResourceList resources;
    private Task task;
    private bool toRequester;
    private bool itemsReserved;

    private GameObject closestSource;
    private Transform destination;

    public bool taskInitiated;
    public int currentTaskStep;

    private float waitTime = 0.5f;
    private float startTime;
    private bool waitInitiated;


	// Use this for initialization
	void Start () {
        routine = GetComponent<WorkerRoutine>();
        stats = GetComponent<WorkerStats>();
        anim = GetComponent<WorkerAnimationManager>();
        path = GetComponent<PathFinding>();
        inventory = GameObject.FindWithTag("GameManager").GetComponent<TotalInventory>();
    }
	
	// Update is called once per frame
	void Update () {


        if (taskInitiated)
        {
            switch (currentTaskStep)
            {
                case 0:
                    SelectPickUpLocation();
                    break;

                case 1:
                    MoveToPickupLocation();
                    break;

                case 2:
                    if (path.GetAtDestination())
                    {
                        ArrivedAtDestination();
                    }
                    break;

                case 3:
                    
                    CollectItems();
                    break;

                case 4:
                    SelectDeliveryLocation();
                    break;

                case 5:
                    MoveToDeliveryLocation();
                    break;

                case 6:
                    if (path.GetAtDestination())
                    {
                        ArrivedAtDestination();
                    }
                    break;

                case 7:
                    
                    DepositTems();
                    break;

                case 8:
                    CompleteTask();
                    break;

                default:
                    Debug.Log("Error: Delivery taskStep out of range");
                    break;
            }

        }
		
	}

    public void StartTask(Task _task, bool _toRequester)
    {
        task = _task;
        resources = task.getResources();       
        toRequester = _toRequester;
        taskInitiated = true;
        currentTaskStep = 0;
    }

    #region task steps
    void SelectPickUpLocation() //step 0
    {
        if (toRequester)
        {
            if (resources.GetWood() > 0)
            {
                if (inventory.GetWoodTotal() >= resources.GetWood())
                {
                    closestSource = FindClosestObjectWithTag("Warehouse");
                    destination = closestSource.transform;
                    ReserveItems(closestSource, resources);
                }
                else //add else if for woodcutter, once implemented
                {
                    closestSource = FindClosestObjectWithTag("Forest");
                    destination = closestSource.transform;
                }
            }
            else if (resources.GetStone() > 0)
            {
                if (inventory.GetStoneTotal() >= resources.GetStone())
                {
                    closestSource = FindClosestObjectWithTag("Warehouse");
                    destination = closestSource.transform;
                    ReserveItems(closestSource, resources);
                }
                else //add else if for stonemason, once implemented
                {
                    closestSource = FindClosestObjectWithTag("Stone");
                    destination = closestSource.transform;
                }
            }
        }
        else
        {
            destination = task.getRequester();
        }

        currentTaskStep++;
    }

    void ReserveItems(GameObject _warehouse, ResourceList _resources) //optional step
    {
        _warehouse.GetComponent<Inventory>().ReserveItem(_resources);
        itemsReserved = true;
    }

    void MoveToPickupLocation() //step 1
    {
        path.MoveToDestination(destination);
        anim.Walking();
        currentTaskStep++;
    }

    void CollectItems() //step 3
    {
        if (!waitInitiated)
        {
            waitInitiated = true;
            startTime = Time.time;
        }
        else if (waitInitiated && Time.time > (startTime + waitTime))
        {
            if (itemsReserved)
            {
                closestSource.GetComponent<Inventory>().CollectReservation(resources);
                stats.AddItemsToInventory(resources);
            }
            else
            {
                closestSource.GetComponent<Inventory>().ModifyResources(resources);
                stats.AddItemsToInventory(resources);
            }

            waitInitiated = false;
            currentTaskStep++;
        }        
    }

    void SelectDeliveryLocation() //step 4
    {
        if (!toRequester)
        {
            closestSource = FindClosestObjectWithTag("Warehouse");
            destination = closestSource.transform;   
        }
        else
        {
            destination = task.getRequester();
        }

        currentTaskStep++;
    }

    void MoveToDeliveryLocation() //step 5
    {
        path.MoveToDestination(destination);
        anim.Walking();
        currentTaskStep++;
    }

    void DepositTems() //step 7
    {
        if (!waitInitiated)
        {
            waitInitiated = true;
            startTime = Time.time;
        }
        else if (waitInitiated && Time.time > (startTime + waitTime))
        {
            if (destination.tag == "Construction")
            {
                stats.ClearInventory();
                destination.GetComponent<Construction>().AddMaterials(resources);
            }
            else
            {
                stats.ClearInventory();
                destination.GetComponent<Inventory>().ModifyResources(resources);
            }

            waitInitiated = false;
            currentTaskStep++;
        }
    }

    void CompleteTask() //step 8
    {
        taskInitiated = false;
        currentTaskStep++;
        routine.SetTaskComplete();
    }

    private void ArrivedAtDestination() //step 2 & 6
    {
        anim.Idle();
        currentTaskStep++;
    }

    #endregion

    private GameObject FindClosestObjectWithTag(string _tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(_tag);
        if (gos.Length == 0)
        {
            return null;
        }
        else
        {
            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
            return closest;
        }
    }
}
