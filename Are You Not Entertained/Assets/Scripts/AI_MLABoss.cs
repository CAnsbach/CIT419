using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class AI_MLABoss : Agent
{
    public override void OnActionReceived(float[] vectorAction)
    {
        base.OnActionReceived(vectorAction);
        Debug.Log(GetAction()[0]);
    }
}
