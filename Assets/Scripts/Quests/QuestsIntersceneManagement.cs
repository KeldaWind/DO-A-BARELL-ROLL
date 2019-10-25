using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestsIntersceneManagement 
{
    [Header("Set Up")]
    [SerializeField] List<QuestSet> allQuestSets = new List<QuestSet>();
    [SerializeField] int currentSetIndex = 0;

    [Space(16)]

    [Header("Progression")]
    [SerializeField] Quest[] currentQuests = new Quest[3];
    public Quest[] GetCurrentQuests { get { return currentQuests; } }

    [SerializeField] List<Quest> finishedQuests = new List<Quest>();

    [SerializeField] List<WeaponParameters> unlockedWeapons = new List<WeaponParameters>();


    public void CheckForFinishedQuests(float travelledDistance, int numberOfKilledEnemies, int numberOfBarellRolls)
    {
        for (int i = 0; i < currentQuests.Length; i++)
        {
            Quest quest = currentQuests[i];

            if (quest == null)
                continue;

            bool questFinished = false;
            switch (quest.questType)
            {
                case QuestType.TravelDistance:
                    questFinished = quest.AddCurrentValue(travelledDistance);
                    break;

                case QuestType.KillEnemies:
                    questFinished = quest.AddCurrentValue(numberOfKilledEnemies);
                    break;

                case QuestType.DoBarellRoll:
                    questFinished = quest.AddCurrentValue(numberOfBarellRolls);
                    break;
            }

            if (questFinished)
            {
                finishedQuests.Add(quest);
                if (quest.rewardWeapon != null)
                    unlockedWeapons.Add(quest.rewardWeapon);

                //currentQuests[i] = null;
            }
        }
    }
    

    public void AssignNewQuests()
    {
        for (int i = 0; i < currentQuests.Length; i++) 
        {
            Quest quest = currentQuests[i];

            if (quest == null)
            {
                while(allQuestSets[currentSetIndex].allSetQuests.Count == 0)
                {
                    currentSetIndex++;
                    if(currentSetIndex == allQuestSets.Count)
                    {
                        Debug.LogWarning("NO MORE QUESTS");
                        return;
                    }
                }

                Quest newQuest = allQuestSets[currentSetIndex].allSetQuests[Random.Range(0, allQuestSets[currentSetIndex].allSetQuests.Count)];

                if (newQuest == null)
                    continue;

                newQuest.ResetCurrentValue();
                currentQuests[i] = newQuest;

                allQuestSets[currentSetIndex].allSetQuests.Remove(newQuest);
            }
        }
    }

    public void RemoveDoneQuests()
    {
        for(int i = 0; i < currentQuests.Length; i++)
        {
            if (currentQuests[i].Done)
            {
                Debug.Log("Remove : " + currentQuests[i].questName);
                currentQuests[i] = null;
            }
        }
    }

    public void ResetQuests()
    {
        foreach (Quest quest in currentQuests)
        {
            if (quest == null)
                break;

            if (quest.questRealisationType == QuestRealisationType.InOneGame)
                quest.ResetCurrentValue();
        }
    }
}

[System.Serializable]
public struct QuestSet
{
    public string questSetName;
    public List<Quest> allSetQuests;
}