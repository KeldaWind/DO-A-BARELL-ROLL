using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinMaxMode
{
    Constant, RandomBetweenMinAndMax
}

[System.Serializable]
public struct MinMaxFloat
{
    public MinMaxMode mode;
    public float minValue;

    public float maxValue;

    public MinMaxFloat(float min, float max)
    {
        mode = MinMaxMode.RandomBetweenMinAndMax;
        minValue = min;
        maxValue = max;
    }

    public float GetValue()
    {
        switch (mode)
        {
            case (MinMaxMode.Constant):
                return minValue;

            case (MinMaxMode.RandomBetweenMinAndMax):
                return Random.Range(minValue, maxValue);

            default: return 0;
        }
    }
}
