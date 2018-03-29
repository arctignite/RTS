using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //added for ToList()

public class Construction : MonoBehaviour {

    public float stoneRequired, woodRequired;
    public float stonePresent, woodPresent;
    public GameObject modelConstruct, modelFinished, materialWood, materialStone, border, building, transparantModel;

    public float requiredBuildTime;
    public float productivity;
    private int colliderCounter;

    public float materialsAvailable;
    public float buildSoFar = 0, buildPercent = 0;

    public bool finished = false;
    private bool constructionTaskCreated;
    private bool constructionStarted;
    private bool unableToBuild;

    private Color borderColorGood, borderColorBad;

    public List<GameObject> activeWorkers = new List<GameObject>();

    public TaskManager taskManager;

    private PlaceConstructionSite placer;

    [SerializeField] private BoxCollider collider;

    void Start()
    {
        taskManager = GameObject.FindWithTag("GameManager").GetComponent<TaskManager>();
        placer = GameObject.FindWithTag("GameManager").GetComponent<PlaceConstructionSite>();
        collider = GetComponent<BoxCollider>();
        borderColorBad = new Color(1, 0.3f, 0.3f, 0.5f);
        borderColorGood = new Color(0.3f, 1, 0.5f, 0.15f);
    }

    // Update is called once per frame
    void Update () {
        materialsAvailable = (stonePresent + woodPresent) / (stoneRequired + woodRequired);

        if (materialsAvailable > buildPercent)
        {
            if (!constructionTaskCreated)
            {
                CreateConstructionTask();
                constructionTaskCreated = true;
            }

            if (productivity > 0)
            {
                Construct();
                SetModel();
            }
        }
        else
        {
            if (constructionTaskCreated)
            {
                StopConstruction();
            }
        }
     
        if (finished)
        {
            Instantiate(building, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.x));
            StopConstruction();
            Destroy(gameObject);
        }
	}

    void Construct()
    {
        buildSoFar += productivity * Time.deltaTime;

        buildPercent = buildSoFar / requiredBuildTime;

        if (buildPercent >= 1)
        {
            finished = true;
        }
    }

    void SetModel() //kan efficienter, maar werkt voor nu.
    {
        if (buildPercent < 0.5f)
        {
            modelConstruct.SetActive(false);
            if (woodPresent > 0)
            {
                materialWood.SetActive(true);
            }

            if (stonePresent > 0)
            {
                materialStone.SetActive(true);
            }
        }else if(buildPercent >= 0.5f && buildPercent < 1f)
        {
            materialWood.SetActive(false);
            materialStone.SetActive(false);
            border.SetActive(false);
            modelConstruct.SetActive(true);
        }
        else
        {
            materialWood.SetActive(false);
            materialStone.SetActive(false);
            modelConstruct.SetActive(false);
            border.SetActive(false);
            modelFinished.SetActive(true);
        }
    }

    public void InitiateBuilding()
    {
        constructionStarted = true;
        transparantModel.SetActive(false);
        collider.isTrigger = false;
        CreateWoodDeliveryTasks();
        CreateStoneDeliveryTasks();
    }

    private void CreateWoodDeliveryTasks()
    {
        for (int i = 0; i < woodRequired; i++)
        {
            taskManager.NewTask(new ResourceList(wood: 1), 1, Time.time, transform, 1);
        }
    }

    private void CreateStoneDeliveryTasks()
    {
        for (int i = 0; i < stoneRequired; i++)
        {
            taskManager.NewTask(new ResourceList(stone: 1), 1, Time.time, transform, 1);
        }
    }

    private void CreateConstructionTask()
    {
        taskManager.NewTask(null, 10, Time.time, transform, 3);
        //add to active build list
    }

    private void StopConstruction()
    {
        foreach (GameObject worker in activeWorkers.ToList())
        {
            worker.GetComponent<ConstructionTask>().StopConstruction();
        }

        constructionTaskCreated = false;
        //remove from active build list
    }

    public void AddMaterials(ResourceList _resources)
    {
        woodPresent += _resources.GetWood();
        stonePresent += _resources.GetStone();
        SetModel();

        if (materialsAvailable > buildPercent && !constructionTaskCreated)
        {
            constructionTaskCreated = true;
            CreateConstructionTask();
        }
    }

    //werkt voor nu, maar liever naar de eigenlijke builders linken
    public void AddBuilder(GameObject _worker)
    {
        activeWorkers.Add(_worker);
        productivity = 0;
        foreach (GameObject worker in activeWorkers)
        {
            productivity += worker.GetComponent<WorkerStats>().GetProductivity();
        }
    }

    public void RemoveBuilder(GameObject _worker)
    {
        activeWorkers.Remove(_worker);
        productivity = 0;
        foreach (GameObject worker in activeWorkers)
        {
            productivity += worker.GetComponent<WorkerStats>().GetProductivity();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("trigger entered");
        if (other.gameObject.layer != LayerMask.NameToLayer("Walkable"))
        {
            colliderCounter++;
            unableToBuild = true;
            placer.SetAbleToBuild(false);
            border.GetComponent<Renderer>().material.color = borderColorBad;
            transparantModel.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Walkable"))
        {
            colliderCounter--;
            if (colliderCounter == 0)
            {
                unableToBuild = false;
                placer.SetAbleToBuild(true);
                border.GetComponent<Renderer>().material.color = borderColorGood;
                transparantModel.SetActive(true);
            }
        }
    }

}
