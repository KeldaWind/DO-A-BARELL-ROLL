using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour, IPoolableObject
{
    int poolingIndex;
    public void SetPoolingIndex(int index) { poolingIndex = index; }
    public int GetPoolingIndex { get { return poolingIndex; } }

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
        gameObject.SetActive(true);
    }

    public System.Action<Obstacle> OnReturnToPool;

    public void SetUpOnPoolInstantiation(int poolIndex)
    {
        gameObject.SetActive(false);
        poolingIndex = poolIndex;
    }

    public void ReturnObjectToPool()
    {
        gameObject.SetActive(false);

        if (OnReturnToPool != null)
            OnReturnToPool(this);
        else
            Destroy(gameObject);
    }
}
