using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceShipMovementSystem
{
    [SerializeField] SpaceShipMovementParameters movementParameters;
    public void SetMovementParameters(SpaceShipMovementParameters parameters)
    {
        movementParameters = parameters;
    }
    public SpaceShipMovementParameters GetMovementParameters { get { return movementParameters; } }

    SpaceShipBaseMovementValues baseMovementValues;
    SpaceShipBarellRollValues barellRollValues;

    public void SetUp()
    {
        baseMovementValues = movementParameters.GetBaseMovementValues;
        barellRollValues = movementParameters.GetBarellRollValues;

        barellRollDurationSystem = new TimerSystem(barellRollValues.GetBarellRollDuration, EndBarellRoll);
        barellRollCooldownSystem = new TimerSystem(barellRollValues.GetBarellRollCooldown, null);
        barellRollCurve = barellRollValues.GetBarellRollCurve;
    }

    public void Reset()
    {
        currentMovementVector = Vector2.zero;
    }

    Vector2 currentMovementVector;
    public Vector2 GetCurrentVelocity { get { return currentMovementVector * Time.deltaTime; } }

    #region Base Movement
    /// <summary>
    /// Updates the values of the movement script and returns the force linked to this movement.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="canMove"></param>
    /// <returns></returns>
    public virtual Vector2 UpdateMovementValuesAndGetForce(Vector2 input)
    {
        if (movementParameters == null)
            return Vector2.zero;

        input.Normalize();

        float targetXSpeed = input.x * baseMovementValues.GetMaxSpeed;
        float targetYSpeed = input.y * baseMovementValues.GetMaxSpeed;

        #region V2       
        #region X
        // Use acceleration if abs(a) needs to be increased
        if (Mathf.Abs(targetXSpeed) > Mathf.Abs(currentMovementVector.x))
        {
            if (Mathf.Abs(currentMovementVector.x - targetXSpeed) < baseMovementValues.GetAcceleration * Time.deltaTime)
                currentMovementVector.x = targetXSpeed;
            else
                currentMovementVector.x += baseMovementValues.GetAcceleration * Mathf.Sign(targetXSpeed) * Time.deltaTime;
        }

        // Use desceleration if abs(a) needs to be decreased
        else if (Mathf.Abs(targetXSpeed) < Mathf.Abs(currentMovementVector.x))
        {
            if (Mathf.Abs(currentMovementVector.x - targetXSpeed) < baseMovementValues.GetDesceleration * Time.deltaTime)
                currentMovementVector.x = targetXSpeed;
            else
                currentMovementVector.x -= baseMovementValues.GetDesceleration * Mathf.Sign(currentMovementVector.x) * Time.deltaTime;
        }
        #endregion

        #region Y
        // Use acceleration if abs(a) needs to be increased
        if (Mathf.Abs(targetYSpeed) > Mathf.Abs(currentMovementVector.y))
        {
            if (Mathf.Abs(currentMovementVector.y - targetYSpeed) < baseMovementValues.GetAcceleration * Time.deltaTime)
                currentMovementVector.y = targetYSpeed;
            else
                currentMovementVector.y += baseMovementValues.GetAcceleration * Mathf.Sign(targetYSpeed) * Time.deltaTime;
        }

        // Use desceleration if abs(a) needs to be decreased
        else if (Mathf.Abs(targetYSpeed) < Mathf.Abs(currentMovementVector.y))
        {
            if (Mathf.Abs(currentMovementVector.y - targetYSpeed) < baseMovementValues.GetDesceleration * Time.deltaTime)
                currentMovementVector.y = targetYSpeed;
            else
                currentMovementVector.y -= baseMovementValues.GetDesceleration * Mathf.Sign(currentMovementVector.y) * Time.deltaTime;
        }
        #endregion
        #endregion

        return currentMovementVector * Time.deltaTime;
    }
    #region Actions
    public void ResetMovementValues()
    {
        currentMovementVector = Vector2.zero;
    }
    #endregion

    #endregion

    #region Barell Roll
    TimerSystem barellRollDurationSystem;
    TimerSystem barellRollCooldownSystem;
    public bool IsBarellRolling { get { return !barellRollDurationSystem.TimerOver; } }
    public bool ReloadingBarellRoll { get { return !barellRollCooldownSystem.TimerOver; } }
    //public Vector3 GetCurrentBarellRollPosition { }
    Vector3 barellRollStartPosition;
    Vector3 barellRollEndPosition;
    AnimationCurve barellRollCurve;
    BarellRollDirection currentBarellRollDirection;

    public System.Action OnBarellRollStart;
    public System.Action OnBarellRollEnd;

    public void StartBarellRoll(BarellRollDirection barellRollDirection, Vector3 startPosition)
    {
        if (!barellRollValues.GetCanDoABarellRool || IsBarellRolling || ReloadingBarellRoll)
            return;

        currentBarellRollDirection = barellRollDirection;

        barellRollStartPosition = startPosition;

        switch (barellRollDirection)
        {
            case (BarellRollDirection.Left):
                barellRollEndPosition = barellRollStartPosition + Vector3.left * barellRollValues.GetBarellRollDistance;
                break;

            case (BarellRollDirection.Right):
                barellRollEndPosition = barellRollStartPosition + Vector3.right * barellRollValues.GetBarellRollDistance;
                break;

            case (BarellRollDirection.Front):
                barellRollEndPosition = barellRollStartPosition + Vector3.up * barellRollValues.GetBarellRollDistance;
                break;
        }

        barellRollDurationSystem.StartTimer();

        OnBarellRollStart?.Invoke();
    }

    public Vector3 GetCurrentBarellRollPosition
    {
        get
        {
            return Vector3.Lerp(barellRollStartPosition, barellRollEndPosition, GetBarellRollProgression);
        }
    }

    public Vector3 UpdateBarellRollAndGetPosition()
    {
        barellRollDurationSystem.UpdateTimer();

        return GetCurrentBarellRollPosition;
    }

    public float GetBarellRollProgression { get { return barellRollCurve.Evaluate(barellRollDurationSystem.GetTimerCoefficient); } }

    public void EndBarellRoll()
    {
        barellRollCooldownSystem.StartTimer();

        switch (currentBarellRollDirection)
        {
            case (BarellRollDirection.Left):
                break;

            case (BarellRollDirection.Right):
                break;

            case (BarellRollDirection.Front):
                break;
        }

        ResetMovementValues();

        OnBarellRollEnd?.Invoke();
    }

    public void UpdateBarellRollCooldown()
    {
        barellRollCooldownSystem.UpdateTimer();
    }
    #endregion
}

public enum BarellRollDirection { None, Left, Right, Front }