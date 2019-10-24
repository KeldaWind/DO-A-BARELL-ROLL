using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipScript : MonoBehaviour
{
    bool setedUp = true;

    #region Movements
    [HideInInspector] [SerializeField] SpaceShipMovementSystem movementSystem = new SpaceShipMovementSystem();
    public SpaceShipMovementSystem GetMovementSystem { get { return movementSystem; } }

    public void UpdateMovements()
    {
        if (!movementSystem.IsBarellRolling)
        {
            Vector2 movementForce = movementSystem.UpdateMovementValuesAndGetForce(GetCurrentMovementInputVector);
            transform.position += (Vector3)movementForce;
        }
        else
        {
            transform.position = movementSystem.UpdateBarellRollAndGetPosition();
        }
    }
    #endregion

    #region Shooting
    [SerializeField] protected ShootingSystem shootingSystem = new ShootingSystem();
    [SerializeField] protected DamageTag damageTag = DamageTag.Enemy;
    public ShootingSystem GetShootingSystem { get { return shootingSystem; } }

    public void UpdateShooting()
    {
        #region Input
        if (StartedShootInput)
            shootingSystem.OnInputPressed();
        else if (IsMaintainingShootInput)
            shootingSystem.OnInputMaintained();
        else if (ReleasedShootInput)
            shootingSystem.OnInputReleased();
        #endregion

        shootingSystem.UpdateSystem();
    }
    #endregion

    #region Inputs
    public virtual Vector2 GetCurrentMovementInputVector { get { return Vector2.zero; } }

    public virtual BarellRollDirection GetBarellRollRequest { get { return BarellRollDirection.None; } }

    public virtual bool StartedShootInput { get { return false; } }
    public virtual bool IsMaintainingShootInput { get { return false; } }
    public virtual bool ReleasedShootInput { get { return false; } }
    #endregion

    #region Damages
    [SerializeField] DamageableComponent relatedDamageableComponent= default;
    public DamageableComponent GetRelatedDamageableComponent { get { return relatedDamageableComponent; } }
    public void SetRelatedDamageableComponent(DamageableComponent component)
    {
        relatedDamageableComponent = component;
    }

    public void DebugLogLifeAmount()
    {
        Debug.Log(relatedDamageableComponent.GetCurrentLifeAmount + "/" + relatedDamageableComponent.GetMaxLifeAmount);
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void SetVulnerable()
    {
        relatedDamageableComponent.gameObject.SetActive(true);
    }
    public void SetInvulnerable()
    {
        relatedDamageableComponent.gameObject.SetActive(false);
    }
    #endregion

    public virtual void SetUp()
    {
        movementSystem.SetUp();
        movementSystem.OnBarellRollStart += SetInvulnerable;
        movementSystem.OnBarellRollEnd += SetVulnerable;

        shootingSystem.SetUp(damageTag);

        relatedDamageableComponent.SetUp(damageTag);
        relatedDamageableComponent.OnLifeAmountChanged += DebugLogLifeAmount;
        relatedDamageableComponent.OnLifeAmountReachedZero += Die;
    }

    public virtual void UpdateSpaceShip()
    {
        #region Movements
        if (GetBarellRollRequest != BarellRollDirection.None)
            movementSystem.StartBarellRoll(GetBarellRollRequest, transform.position);

        UpdateMovements();

        if (movementSystem.ReloadingBarellRoll)
            movementSystem.UpdateBarellRollCooldown();
        #endregion

        UpdateShooting();
    }


    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        UpdateSpaceShip();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Wall>() != null)
            Die();
    }
}
