using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "DO A BARELL ROLL/Scriptables/Quests/New Quest")]
public class Quest : ScriptableObject
{
    public void SetUpWithTab(string name, string description, QuestType type, QuestRealisationType realisationType, float valToReach)
    {
        questName = name;
        questDescription = description;
        questType = type;
        questRealisationType = realisationType;
        valueToReach = valToReach;
    }

    public string questName = "New Quest";
    public string questDescription = "Here's a new Quest !";
    public QuestType questType = QuestType.TravelDistance;
    public QuestRealisationType questRealisationType =  QuestRealisationType.InOneGame;
    public float valueToReach = 50;

    float currentValue = 0;
    public float GetCurrentValue { get { return currentValue; } }

    public bool Done { get { return currentValue >= valueToReach; } }

    public WeaponParameters rewardWeapon;

    public bool AddCurrentValue(float addedValue)
    {
        currentValue += addedValue;

        return currentValue >= valueToReach;
    }

    public void ResetCurrentValue()
    {
        currentValue = 0;
    }
}

public enum QuestType { TravelDistance, KillEnemies, DoBarellRoll }
public enum QuestRealisationType { InOneGame, OverMultipleGames }