using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableComponent : MonoBehaviour
{
    [SerializeField] int maxLife = 100;
    DamageTag damageTag = DamageTag.Environment;
    public DamageTag GetDamageTag { get { return damageTag; } }

    public int GetMaxLifeAmount { get { return maxLife; } }
    public void SetMaxLifeAmount(int amount) { maxLife = amount; }

    int currentLifeAmount;
    public int GetCurrentLifeAmount { get { return currentLifeAmount; } }

    public System.Action OnLifeAmountChanged;
    public System.Action OnLifeAmountReachedZero;

    public void SetUp(DamageTag dmgTag)
    {
        currentLifeAmount = maxLife;
        damageTag = dmgTag;
    }

    public void ResetValues()
    {
        currentLifeAmount = maxLife;
    }

    public void Damage(int damageValue)
    {
        currentLifeAmount -= Mathf.Abs(damageValue);

        OnLifeAmountChanged?.Invoke();

        if(currentLifeAmount <=0)
        {
            currentLifeAmount = 0;
            OnLifeAmountReachedZero?.Invoke();
        }

    }
}

public enum DamageTag { Player, Enemy, Environment }