using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStoneAction : GoapAction {

    private bool hasStone = false;
    private StoneDepositComponent targetStoneDeposit;

    public CollectStoneAction()
    {
        addPrecondition("hasStone", false);
        addEffect("hasStone", true);
    }

    public override void reset()
    {
        hasStone = false;
        targetStoneDeposit = null;
    }

    public override bool isDone()
    {
        return hasStone;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        //find nearest deposit:
        StoneDepositComponent[] stoneDeposits = (StoneDepositComponent[])FindObjectsOfType(typeof(StoneDepositComponent));
        StoneDepositComponent closest = null;
        float closestDist = 0;

        foreach (StoneDepositComponent stoneDeposit in stoneDeposits)
        {
            if (stoneDeposit.numStone > 0)
            {
                if (closest == null)
                {
                    closest = stoneDeposit;
                    closestDist = (stoneDeposit.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (stoneDeposit.gameObject.transform.position - agent.transform.position).magnitude;
                    if (dist < closestDist)
                    {
                        closest = stoneDeposit;
                        closestDist = dist;
                    }
                }
            }          
        }

        if (closest == null)
        {
            return false;
        }

        targetStoneDeposit = closest;
        target = targetStoneDeposit.gameObject;

        return closest != null;
    }

    public override bool perform(GameObject agent)
    {
        
        if (targetStoneDeposit.numStone > 0)
        {
            int carryingCapacity = agent.GetComponent<MinionStatsComponent>().carryingCapacity;

            InventoryComponent inventory = agent.GetComponent<InventoryComponent>();

            if (targetStoneDeposit.numStone >= carryingCapacity)
            {
                targetStoneDeposit.numStone -= carryingCapacity;
                inventory.numStone += carryingCapacity;
            }
            else
            {
                inventory.numStone += targetStoneDeposit.numStone; ;
                targetStoneDeposit.numStone = 0;
            }
            
            hasStone = true;
            //TODO: add animation for stonecarrying
            
            return true;
        }
        else
        {
            return false;
        }
    }
}
