using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [SerializeField] private float wood;
    [SerializeField] private float stone;

    [SerializeField] private float reservedWood;
    [SerializeField] private float reservedStone;

    [SerializeField] private string name;

    private TotalInventory totalInventory;
    private string tagSelf;

    private void Start()
    {
        totalInventory = GameObject.FindWithTag("GameManager").GetComponent<TotalInventory>();
        tagSelf = transform.tag;
        name = gameObject.name;
    }

    public float GetWood()
    {
        return wood;
    }

    public void ModifyResources(ResourceList _resources, bool _add = true)
    {
        if (_add)
        {
            wood += _resources.GetWood();
            stone += _resources.GetStone();
        }
        else
        {
            wood -= _resources.GetWood();
            stone -= _resources.GetStone();
        }

        if (tagSelf == "Warehouse")
        {
            totalInventory.ModifyResourceTotal(_resources, _add);
        }
    }

    public float GetStone()
    {
        return stone;
    }

    public void ReserveItem(ResourceList _resources)
    {
        reservedStone += _resources.GetStone();
        reservedWood += _resources.GetWood();

        ModifyResources(_resources, false);
    }

    public void CollectReservation(ResourceList _resources)
    {
        reservedWood -= _resources.GetWood();
        reservedStone -= _resources.GetStone();
    }

    public string GetName()
    {
        return name;
    }
}
