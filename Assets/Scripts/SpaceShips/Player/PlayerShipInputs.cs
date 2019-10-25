using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Ship Inputs", menuName = "DO A BARELL ROLL/Scriptables/Player/Inputs")]
public class PlayerShipInputs : ScriptableObject
{
    [Header("Movement Inputs")]
    [SerializeField] string horizontalAxis = "Horizontal";
    public string GetHorizontalAxis { get { return horizontalAxis; } }

    [SerializeField] string verticalAxis = "Vertical";
    public string GetVerticalAxis { get { return verticalAxis; } }

    [Header("Barell Roll Inputs")]
    [SerializeField] KeyCode leftBarellRollKey = KeyCode.LeftArrow;
    public KeyCode GetLeftBarellRollKey { get { return leftBarellRollKey; } }

    [SerializeField] KeyCode rightBarellRollKey = KeyCode.RightArrow;
    public KeyCode GetRightBarellRollKey { get { return rightBarellRollKey; } }
    
    [SerializeField] KeyCode frontBarellRollKey = KeyCode.UpArrow;
    public KeyCode GetFrontBarellRollKey { get { return frontBarellRollKey; } }

    [Header("Weapons")]
    [SerializeField] KeyCode shootKey = KeyCode.Space;
    public KeyCode GetShootKey { get { return shootKey; } }

    [SerializeField] KeyCode nextWeaponKey = KeyCode.A;
    public KeyCode GetNextWeaponKey { get { return nextWeaponKey; } }

    [SerializeField] KeyCode previousWeaponKey = KeyCode.E;
    public KeyCode GetPrevioustWeaponKey { get { return previousWeaponKey; } }
}
