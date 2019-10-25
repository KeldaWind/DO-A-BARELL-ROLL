using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceShipScript : SpaceShipScript
{
    float maxReachedY;
    float startY;

    public void UpdateTravelledDistance()
    {
        if(transform.position.y > maxReachedY)
        {
            maxReachedY = transform.position.y;
            GameManager.gameManager.GetQuestCheckingManager.UpdateTravelledDistance(maxReachedY - startY);
        }
    }

    [HideInInspector] [SerializeField] PlayerShipInputs playerShipInputs = default;
    public PlayerShipInputs GetPlayerShipInputs { get { return playerShipInputs; } }
    public void SetPlayerShipInputs(PlayerShipInputs inputs)
    {
        playerShipInputs = inputs;
    }

    #region Inputs
    public override Vector2 GetCurrentMovementInputVector
    {
        get
        {
            return new Vector2(Input.GetAxis(playerShipInputs.GetHorizontalAxis), Input.GetAxis(playerShipInputs.GetVerticalAxis));
        }
    }

    public override BarellRollDirection GetBarellRollRequest
    {
        get
        {
            if (Input.GetKeyDown(playerShipInputs.GetLeftBarellRollKey))
                return BarellRollDirection.Left;
            else if (Input.GetKeyDown(playerShipInputs.GetRightBarellRollKey))
                return BarellRollDirection.Right;
            else if (Input.GetKeyDown(playerShipInputs.GetFrontBarellRollKey))
                return BarellRollDirection.Front;
            else
                return BarellRollDirection.None;
        }
    }

    public override bool StartedShootInput { get { return Input.GetKeyDown(playerShipInputs.GetShootKey); } }
    public override bool IsMaintainingShootInput { get { return Input.GetKey(playerShipInputs.GetShootKey); } }
    public override bool ReleasedShootInput { get { return Input.GetKeyUp(playerShipInputs.GetShootKey); } }

    public override bool PressedNextWeapon { get { return Input.GetKeyDown(playerShipInputs.GetNextWeaponKey); } }
    public override bool PressedPreviousWeapon { get { return Input.GetKeyDown(playerShipInputs.GetPrevioustWeaponKey); } }
    #endregion

    public override void FirstSetUp()
    {
        base.FirstSetUp();

        shootingSystem.InstantiateBaseWeapon();

        GetMovementSystem.OnBarellRollEnd += GameManager.gameManager.GetQuestCheckingManager.IncreamentNumberOfBarellRoll;

        startY = transform.position.y;
        maxReachedY = startY;
    }

    public override void UpdateSpaceShip()
    {
        base.UpdateSpaceShip();

        UpdateTravelledDistance();
    }

    public override void Die()
    {
        base.Die();
        GameManager.gameManager.GameOver();
    }

}
