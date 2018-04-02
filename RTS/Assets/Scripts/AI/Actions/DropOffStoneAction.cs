using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffStoneAction : GoapAction {

    private bool droppedOffStone = false;
    private StorageComponent targetStorageLocation;

	public DropOffStoneAction()
    {
        addPrecondition("hasStone", true);
        addEffect("hasStone", false);
        addEffect("collectStone", true);
    }

    public override void reset()
    {
        droppedOffStone = false;
        targetStorageLocation = null;
    }

    public override bool isDone()
    {
        return droppedOffStone;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        // find the nearest supply pile that has spare ore
        StorageComponent[] storageLocations = (StorageComponent[])FindObjectsOfType(typeof(StorageComponent));
        StorageComponent closest = null;
        float closestDist = 0;

        foreach (StorageComponent storage in storageLocations)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                closest = storage;
                closestDist = (storage.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (storage.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    closest = storage;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
        {
            return false;
        }
            

        targetStorageLocation = closest;
        target = targetStorageLocation.gameObject;

        return closest != null;
    }

    public override bool perform(GameObject agent)
    {
        InventoryComponent inventory = agent.GetComponent<InventoryComponent>();
        targetStorageLocation.numStone += inventory.numStone;
        droppedOffStone = true;
        inventory.numStone = 0;
        //TODO: add animation etc.

        return true;
    }
}
