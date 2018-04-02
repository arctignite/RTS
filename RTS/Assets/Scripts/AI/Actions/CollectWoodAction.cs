using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectWoodAction : GoapAction {

    private bool hasWood = false;
    private WoodDepositComponent targetWoodDeposit;

    public CollectWoodAction()
    {
        addPrecondition("hasWood", false);
        addEffect("hasWood", true);
    }

    public override void reset()
    {
        hasWood = false;
        targetWoodDeposit = null;
    }

    public override bool isDone()
    {
        return hasWood;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        //find nearest deposit:
        WoodDepositComponent[] woodDeposits = (WoodDepositComponent[])FindObjectsOfType(typeof(WoodDepositComponent));
        WoodDepositComponent closest = null;
        float closestDist = 0;

        foreach (WoodDepositComponent woodDeposit in woodDeposits)
        {
            if (woodDeposit.numWood > 0)
            {
                if (closest == null)
                {
                    closest = woodDeposit;
                    closestDist = (woodDeposit.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (woodDeposit.gameObject.transform.position - agent.transform.position).magnitude;
                    if (dist < closestDist)
                    {
                        closest = woodDeposit;
                        closestDist = dist;
                    }
                }
            }
        }

        if (closest == null)
        {
            return false;
        }

        targetWoodDeposit = closest;
        target = targetWoodDeposit.gameObject;

        return closest != null;
    }

    public override bool perform(GameObject agent)
    {

        if (targetWoodDeposit.numWood > 0)
        {
            int carryingCapacity = agent.GetComponent<MinionStatsComponent>().carryingCapacity;

            InventoryComponent inventory = agent.GetComponent<InventoryComponent>();

            if (targetWoodDeposit.numWood >= carryingCapacity)
            {
                targetWoodDeposit.numWood -= carryingCapacity;
                inventory.numWood += carryingCapacity;
            }
            else
            {
                inventory.numWood += targetWoodDeposit.numWood; ;
                targetWoodDeposit.numWood = 0;
            }

            hasWood = true;
            //TODO: add animation for  wood carrying

            return true;
        }
        else
        {
            return false;
        }
    }
}
