using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceShipScript : SpaceShipScript, IPoolableObject
{
    [HideInInspector] [SerializeField] int poolingIndex;
    public void SetPoolingIndex(int index) { poolingIndex = index; }
    public int GetPoolingIndex { get { return poolingIndex; } }

    PlayerSpaceShipScript targetPlayer;

    [SerializeField] EnemyAimingType aimingType;
    public void SetAimingType(EnemyAimingType aiming) { aimingType = aiming; }

    #region Inputs Simulation
    public override bool IsMaintainingShootInput { get { return enteredScreen && shootingSystem.CanShoot; } }
    #endregion   

    public override void FirstSetUp()
    {
        base.FirstSetUp();

        if (aimingType == EnemyAimingType.AimTowardPlayer)
            targetPlayer = GameManager.gameManager.GetPlayer;

        screenKillTransform = GameManager.gameManager.GetGameScroller.GetKillTransform;
        screenEnterTransform = GameManager.gameManager.GetGameScroller.GetEnterTransform;

        GameManager.gameManager.AddActionToOnGameOver(StopBehaviour);
    }

    public override void ResetValues()
    {
        base.ResetValues();

        enteredScreen = false;
        gameObject.SetActive(true);
    }

    public override void UpdateSpaceShip()
    {
        if (behaviourStopped)
            return;

        if (!enteredScreen)
            enteredScreen = CheckIfEnteredScreen();

        if (enteredScreen)
            UpdateEnemyBehaviour();

        base.UpdateSpaceShip();

        if(CheckIfScreenKill())
            ReturnObjectToPool();
    }

    public void UpdateEnemyBehaviour()
    {
        if (targetPlayer != null)
            AimForPlayer();
    }

    public void AimForPlayer()
    {
        Vector3 lookingDir = (targetPlayer.transform.position - transform.position).normalized;

        float rotZ = Mathf.Atan2(lookingDir.y, lookingDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotZ - 90));
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

    public void SetUpOnPoolInstantiation(int poolIndex)
    {
        shootingSystem.InstantiateBaseWeapon();
        gameObject.SetActive(false);
        poolingIndex = poolIndex;
    }

    public void ResetPoolableObject()
    {
        ResetValues();
    }


    public void ReturnObjectToPool()
    {
        gameObject.SetActive(false);
        if (OnReturnToPool != null)
            OnReturnToPool(this);
        else
            Destroy(gameObject);
    }

    public System.Action<EnemySpaceShipScript> OnReturnToPool;

    public override void Die()
    {
        ReturnObjectToPool();
        GameManager.gameManager.GetQuestCheckingManager.IncreamentNumberOfKilledEnemies();
    }

    bool behaviourStopped;
    public void StopBehaviour()
    {
        behaviourStopped = true;
    }
}

public enum EnemyAimingType { NoAiming, AimTowardPlayer }