using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalInventory : MonoBehaviour {

    [SerializeField] private float totalWoodInWarehouse, totalStoneInWarehouse;

    public void ModifyResourceTotal(ResourceList _resources, bool _add = true)
    {
        if (_add)
        {
            totalWoodInWarehouse += _resources.GetWood();
            totalStoneInWarehouse += _resources.GetStone();
        }
        else
        {
            totalWoodInWarehouse -= _resources.GetWood();
            totalStoneInWarehouse -= _resources.GetStone();
        }
    }

        public float GetWoodTotal()
    {
        return totalWoodInWarehouse;
    }

    public float GetStoneTotal()
    {
        return totalStoneInWarehouse;
    }
}
