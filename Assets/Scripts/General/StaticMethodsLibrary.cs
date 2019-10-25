using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMethodsLibrary : MonoBehaviour
{
    public static float FloatToSingleNumberAfterComma(float baseNumber)
    {
        int tempValue = (int)(baseNumber * 10);

        return (float)tempValue / 10;
    }
}
