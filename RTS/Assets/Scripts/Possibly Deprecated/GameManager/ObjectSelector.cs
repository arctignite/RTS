using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSelector : MonoBehaviour {

    private GameObject selectedObject;
    public GameObject minionUI, buildingUI;
    public Text minionSpeed, minionProductivity, minionWood, minionStone, minionName;
    public Text buildingWood, buildingStone, buildingName;


    private WorkerStats workerStats;
    private Inventory inventory;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelSelection();
        }
    }

    private void CancelSelection()
    {
        selectedObject = null;
        buildingUI.SetActive(false);
        minionUI.SetActive(false);
    }

    public void SelectMinion(GameObject _minion)
    {
        buildingUI.SetActive(false);
        selectedObject = _minion;
        workerStats = selectedObject.GetComponent<WorkerStats>();

        minionName.text = workerStats.GetName();
        minionSpeed.text = workerStats.GetMovementSpeed().ToString();
        minionProductivity.text = workerStats.GetProductivity().ToString();
        minionWood.text = workerStats.GetWood().ToString();
        minionStone.text = workerStats.GetStone().ToString();

        minionUI.SetActive(true);
    }

    public void SelectBuilding(GameObject _building)
    {
        minionUI.SetActive(false);
        selectedObject = _building;
        inventory = selectedObject.GetComponent<Inventory>();

        buildingName.text = inventory.GetName();
        buildingWood.text = inventory.GetWood().ToString();
        buildingStone.text = inventory.GetStone().ToString();

        buildingUI.SetActive(true);
    }

    public void SetMinionType(int _type)
    {
        if (selectedObject != null)
        {
            selectedObject.GetComponent<WorkerStats>().SetMinionType(_type);
        }
    }
}
