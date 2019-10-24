using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceShipScript : SpaceShipScript
{
    //[Header("Inputs")]
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
    #endregion
}
