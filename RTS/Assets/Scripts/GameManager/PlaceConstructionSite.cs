using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceConstructionSite : MonoBehaviour {

    [SerializeField] private GameObject warehousePrefab;
    [SerializeField] private GameObject wellPrefab;
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject sawmillPrefab;
    [SerializeField] private GameObject minePrefab;


    private GameObject currentPlaceableObject;

    private float mouseWheelRotation;
    private bool ableToBuild;
    private int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Walkable");
    }

    void Update () {

        SelectPrefab();

        if (currentPlaceableObject != null)
        {
            MoveObjectToPointer();
            RotateFromMouseWheel();
            if (ableToBuild)
            {
                ReleaseOnMouseUp();
            }         
        }
    }

    private void SelectPrefab()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }

            ableToBuild = true;
            currentPlaceableObject = Instantiate(warehousePrefab);          
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }

            ableToBuild = true;
            currentPlaceableObject = Instantiate(sawmillPrefab);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }

            ableToBuild = true;
            currentPlaceableObject = Instantiate(minePrefab);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }

            ableToBuild = true;
            currentPlaceableObject = Instantiate(wellPrefab);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }

            ableToBuild = true;
            currentPlaceableObject = Instantiate(housePrefab);
        }
    }

    public void SetAbleToBuild(bool _ableToBuild)
    {
        ableToBuild = _ableToBuild;
    }

    private void MoveObjectToPointer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100, layerMask))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    private void ReleaseOnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            currentPlaceableObject.GetComponent<Construction>().InitiateBuilding();
            currentPlaceableObject = null;
        }
    }
}
