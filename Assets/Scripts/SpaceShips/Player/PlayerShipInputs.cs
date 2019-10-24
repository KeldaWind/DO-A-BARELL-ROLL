using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Ship Inputs", menuName = "DO A BARELL ROLL/Player/Inputs")]
public class PlayerShipInputs : ScriptableObject
{
    [Header("Movement Inputs")]
    [SerializeField] string horizontalAxis = "Horizontal";
    public string GetHorizontalAxis { get { return horizontalAxis; } }

    [SerializeField] string verticalAxis = "Vertical";
    public string GetVerticalAxis { get { return verticalAxis; } }

    [Header("Barell Roll Inputs")]
    [SerializeField] KeyCode leftBarellRollKey = KeyCode.A;
    public KeyCode GetLeftBarellRollKey { get { return leftBarellRollKey; } }

    [SerializeField] KeyCode rightBarellRollKey = KeyCode.E;
    public KeyCode GetRightBarellRollKey { get { return rightBarellRollKey; } }
    
    [SerializeField] KeyCode frontBarellRollKey = KeyCode.Space;
    public KeyCode GetFrontBarellRollKey { get { return frontBarellRollKey; } }

    [Header("Weapons")]
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode GetShootKey { get { return shootKey; } }
}
