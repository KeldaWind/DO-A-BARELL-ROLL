using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IPoolableObject
{
    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (CheckIfScreenKill())
            ReturnObjectToPool();
    }

    public void SetUp()
    {
        screenKillTransform = GameManager.gameManager.GetGameScroller.GetKillTransform;
        screenEnterTransform = GameManager.gameManager.GetGameScroller.GetEnterTransform;
    }

    Transform screenEnterTransform;
    Transform screenKillTransform;

    bool enteredScreen = false;
    public bool CheckIfEnteredScreen()
    {
        return transform.position.y < screenEnterTransform.transform.position.y;
    }

    public bool CheckIfScreenKill()
    {
        return transform.position.y < screenKillTransform.transform.position.y;
    }

    public void ResetPoolableObject()
    {
        
    }

    public void ReturnObjectToPool()
    {
        Destroy(gameObject);
    }
}
