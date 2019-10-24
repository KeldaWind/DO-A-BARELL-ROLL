using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Space Ship Movement Parameters", menuName = "DO A BARELL ROLL/SpaceShips/Movement Parameters")]
public class SpaceShipMovementParameters : ScriptableObject
{
    [SerializeField] SpaceShipBaseMovementValues baseMovementValues = new SpaceShipBaseMovementValues(5f);
    public SpaceShipBaseMovementValues GetBaseMovementValues { get { return baseMovementValues; } }

    [Space(16)]

    [SerializeField] SpaceShipBarellRollValues barellRollValues = new SpaceShipBarellRollValues(false, 0.5f, 3f, 0.3f);
    public SpaceShipBarellRollValues GetBarellRollValues { get { return barellRollValues; } }
}

[System.Serializable]
public struct SpaceShipBaseMovementValues
{
    public SpaceShipBaseMovementValues(float mxSpeed)
    {
        maxSpeed = mxSpeed;
        acceleration = mxSpeed * 4;
        desceleration = acceleration;
    }

    [SerializeField] float maxSpeed;
    public float GetMaxSpeed { get { return maxSpeed; } }

    [SerializeField] float acceleration;
    public float GetAcceleration { get { return acceleration; } }

    [SerializeField] float desceleration;
    public float GetDesceleration { get { return desceleration; } }
}

[System.Serializable]
public struct SpaceShipBarellRollValues
{
    public SpaceShipBarellRollValues(bool canDo, float cooldown, float distance, float duration)
    {
        canDoABarellRool = canDo;
        barellRollCooldown = cooldown;
        barellRollDistance = distance;
        barellRollDuration = duration;
        barellRollCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

    [SerializeField] bool canDoABarellRool;
    public bool GetCanDoABarellRool { get { return canDoABarellRool; } }

    [SerializeField] float barellRollCooldown;
    public float GetBarellRollCooldown { get { return barellRollCooldown; } }

    [SerializeField] float barellRollDistance;
    public float GetBarellRollDistance { get { return barellRollDistance; } }

    [SerializeField] float barellRollDuration;
    public float GetBarellRollDuration { get { return barellRollDuration; } }

    [SerializeField] AnimationCurve barellRollCurve;
    public AnimationCurve GetBarellRollCurve { get { return barellRollCurve; } }
}