using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCheckingManager : MonoBehaviour
{
    int numberOfKilledEnemies;
    float travelledDistance;
    int numberOfBarellRoll;

    public void IncreamentNumberOfBarellRoll()
    {
        numberOfBarellRoll++;
    }

    public void IncreamentNumberOfKilledEnemies()
    {
        numberOfKilledEnemies++;
    }

    public void UpdateTravelledDistance(float travelledDist)
    {
        travelledDistance = travelledDist;
    }

    public void DebugStats()
    {
        Debug.Log("Number of Killed Enemies : " + numberOfKilledEnemies);
        Debug.Log("Travelled Distance : " + travelledDistance);
        Debug.Log("Number of Barell Rolls : " + numberOfBarellRoll);
    }
}
