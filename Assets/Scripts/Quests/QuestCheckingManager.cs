using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCheckingManager : MonoBehaviour
{
    int numberOfKilledEnemies;
    public int GetNumberOfKilledEnemies { get { return numberOfKilledEnemies; } }

    float travelledDistance;
    public float GetTavelledDistance { get { return travelledDistance; } }

    int numberOfBarellRoll;
    public int GetNumberOfBarellRoll { get { return numberOfBarellRoll; } }

    public void IncreamentNumberOfBarellRoll()
    {
        numberOfBarellRoll++;
        OnValuesChanged?.Invoke(travelledDistance, numberOfKilledEnemies, numberOfBarellRoll);
    }

    public void IncreamentNumberOfKilledEnemies()
    {
        numberOfKilledEnemies++;
        OnValuesChanged?.Invoke(travelledDistance, numberOfKilledEnemies, numberOfBarellRoll);
    }

    public void UpdateTravelledDistance(float travelledDist)
    {
        travelledDistance = travelledDist;
        OnValuesChanged?.Invoke(travelledDistance, numberOfKilledEnemies, numberOfBarellRoll);
    }

    public System.Action<float, int, int> OnValuesChanged;

    public void DebugStats()
    {
        Debug.Log("Number of Killed Enemies : " + numberOfKilledEnemies);
        Debug.Log("Travelled Distance : " + travelledDistance);
        Debug.Log("Number of Barell Rolls : " + numberOfBarellRoll);
    }
}
