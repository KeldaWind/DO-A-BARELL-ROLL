using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DO A BARELL ROLL/Scriptables/Quests/Quests Library")]
public class QuestsLibrary : ScriptableObject
{
    [SerializeField] List<QuestSet> allQuestSets = new List<QuestSet>();
    public List<QuestSet> GetQuestsSetsCopy
    {
        get
        {
            List<QuestSet> setsCopy = new List<QuestSet>();

            foreach(QuestSet setToCopy in allQuestSets)
                setsCopy.Add(new QuestSet(setToCopy));

            return setsCopy;
        }
    }

    public void AddToLibrary(Quest newQuest, string setName)
    {
        bool alreadyExistingSet = false;

        
        for (int i = 0; i < allQuestSets.Count; i++) 
        {
            QuestSet questSet = allQuestSets[i];

            if (questSet.questSetName == setName)
            {
                alreadyExistingSet = true;

                // Ici, j'ai essayé de faire en sorte que mes quêtes s'associent automatiquement, sans succès
                /*if (questSet.allSetQuests == null)
                                   questSet.allSetQuests = new List<Quest>();

                               if (questSet.allSetQuests.Contains(newQuest))
                                   break;

                               allQuestSets[i].allSetQuests.Add(newQuest);
                               break;*/
            }
        }

        if (!alreadyExistingSet)
        {
            QuestSet newQuestSet = new QuestSet(setName);
            // Ici, j'ai essayé de faire en sorte que mes quêtes s'associent automatiquement, sans succès
            //newQuestSet.allSetQuests.Add(newQuest);

            allQuestSets.Add(newQuestSet);
        }
    }
}


[System.Serializable]
public struct QuestSet
{
    public QuestSet(string name)
    {
        questSetName = name;
        allSetQuests = new List<Quest>();
    }

    public string questSetName;
    public List<Quest> allSetQuests;

    public QuestSet(QuestSet toCopy)
    {
        questSetName = toCopy.questSetName;
        allSetQuests = new List<Quest>(toCopy.allSetQuests);
    }
}