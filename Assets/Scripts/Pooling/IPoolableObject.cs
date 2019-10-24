using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject 
{
    bool CheckIfEnteredScreen();

    bool CheckIfScreenKill();

    void ResetPoolableObject();

    void ReturnObjectToPool();

}
