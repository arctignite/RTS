using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerController : MonoBehaviour {

    public Animator anim;
    public GameObject axe, pickaxe, hammer;
    public bool carryingWood, carryingBag, attacking, walking;

    public Task activeTask;
    public TaskManager taskManager;
    public TotalInventory totalInventory;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        taskManager = GameObject.FindWithTag("GameManager").GetComponent<TaskManager>();
        totalInventory = GameObject.FindWithTag("GameManager").GetComponent<TotalInventory>();
        activeTask = null;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1)) //carry nothing
        {
            anim.SetBool("carryingWood", false);
            anim.SetBool("carryingBag", false);
            carryingBag = false;
            carryingWood = false;
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //carry wood
        {
            anim.SetBool("carryingWood", true);
            anim.SetBool("carryingBag", false);
            carryingBag = false;
            carryingWood = true;
            axe.SetActive(false);
            pickaxe.SetActive(false);
            hammer.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //carry sack
        {
            anim.SetBool("carryingWood", false);
            anim.SetBool("carryingBag", true);
            carryingBag = true;
            carryingWood = false;
            axe.SetActive(false);
            pickaxe.SetActive(false);
            hammer.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7)) //equip axe
        {
            axe.SetActive(true);
            pickaxe.SetActive(false);
            hammer.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8)) //equip pickaxe
        {
            axe.SetActive(false);
            pickaxe.SetActive(true);
            hammer.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9)) //equip hammer
        {
            axe.SetActive(false);
            pickaxe.SetActive(false);
            hammer.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.A)) //attack toggle
        {
            if (attacking)
            {
                anim.SetBool("attacking", false);
                attacking = false;
            }
            else
            {
                anim.SetBool("attacking", true);
                anim.SetBool("walking", false);
                attacking = true;
                walking = false;
            }                      
        }

        /*if (Input.GetKeyDown(KeyCode.S)) //walk toggle
        {
            if (walking)
            {
                anim.SetBool("walking", false);
                walking = false;

            }
            else
            {
                anim.SetBool("walking", true);
                anim.SetBool("attacking", false);
                walking = true;
                attacking = false;
            }
        }*/
  
        if (Input.GetKeyDown(KeyCode.I)) //assign a task
        {
            
            if (activeTask == null)
            {
                activeTask = taskManager.AssignTask();
                Debug.Log("assigning task");
            }
            else
            {
                Debug.Log("active task isn't null");
            }         
        }

        if (Input.GetKeyDown(KeyCode.O)) //complete a task
        {
            Debug.Log("task complete");
            activeTask = null;
        }

        if (Input.GetKeyDown(KeyCode.P)) //return task to queue
        {
            if (activeTask != null)
            {
                taskManager.ReturnTask(activeTask);
                activeTask = null;
            }            
        }
    }

    private void CollectStoneTask()
    {
        if (totalInventory.GetStoneTotal() <= 0)
        {
            //collect stone from mine
        }
        else
        {
            //collect stone from warehouse
        }
    }

    private void CollectWoodTask()
    {
        if (totalInventory.GetWoodTotal() <= 0)
        {
            //collect wood from trees
        }
        else
        {
            //collect stone from warehouse
        }
    }
}
