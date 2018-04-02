using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCutter : Minion {

    //Set goal, which will be to collect stone

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("collectWood", true));
        return goal;
    }
}
