using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceShipScript : SpaceShipScript, IPoolableObject
{
    PlayerSpaceShipScript targetPlayer;

    [SerializeField] EnemyAimingType aimingType;
    public void SetAimingType(EnemyAimingType aiming) { aimingType = aiming; }

    #region Inputs Simulation
    public override bool IsMaintainingShootInput { get { return enteredScreen && shootingSystem.CanShoot; } }
    #endregion
    
    public override void SetUp()
    {
        base.SetUp();

        enteredScreen = false;

        if (aimingType == EnemyAimingType.AimTowardPlayer)
            targetPlayer = GameManager.gameManager.GetPlayer;

        screenKillTransform = GameManager.gameManager.GetGameScroller.GetKillTransform;
        screenEnterTransform = GameManager.gameManager.GetGameScroller.GetEnterTransform;
    }

    public override void UpdateSpaceShip()
    {
        if (!enteredScreen)
            enteredScreen = CheckIfEnteredScreen();

        if (enteredScreen)
            UpdateEnemyBehaviour();

        base.UpdateSpaceShip();

        if(CheckIfScreenKill())
            Die();
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

    public void ResetPoolableObject()
    {
        SetUp();
    }

    public void ReturnObjectToPool()
    {
        Destroy(gameObject);
    }

    public override void Die()
    {
        Debug.Log("Need to return to pool");
        base.Die();
    }
}

public enum EnemyAimingType { NoAiming, AimTowardPlayer }