using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Start Screen")]
    [SerializeField] GameObject startPanel = default;
    public void ShowStartPanel()
    {
        startPanel.SetActive(true);
    }

    public void HideStartPanel()
    {
        startPanel.SetActive(false);
    }

    [Header("Game Over Screen")]
    [SerializeField] GameObject gameOverPanel = default;
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }
    [Header("LifeBar")]
    [SerializeField] Image barFill = default;
    [SerializeField] Gradient lifeBarColor = default;

    public void UpdateLifeBarFillAmount(int changeValue, int currentLifeAmount, int maxLifeAmount)
    {
        float lifeCoeff = (float)currentLifeAmount / maxLifeAmount;
        barFill.fillAmount = lifeCoeff;
        barFill.color = lifeBarColor.Evaluate(lifeCoeff);
    }

    [Header("Weapon")]
    [SerializeField] Text weaponText = default;
    public void UpdateWeaponText(string selectedWeaponName)
    {
        weaponText.text = "Weapon : " + selectedWeaponName;
    }

    [Header("Progression")]
    [SerializeField] Text distanceText = default;
    [SerializeField] Text destroyedEnemiesText = default;

    public void UpdateProgressionTexts(float travelledDistance, int numberOfKilledEnemies, int numberOfBarellRolls)
    {
        distanceText.text = "Travelled Distance :\n" + StaticMethodsLibrary.FloatToSingleNumberAfterComma(travelledDistance) + " km";
        destroyedEnemiesText.text = "Killed Enemies :\n" + numberOfKilledEnemies;
    }

    [Header("Quests")]
    [SerializeField] QuestFrameUI[] questsUI = new QuestFrameUI[3];

    public void OpenQuestsFrames(Quest[] quests)
    {
        foreach (QuestFrameUI questFrame in questsUI)
            questFrame.ShowPanel();

        Quest currentQuestToShow = null;

        for(int i = 0; i < quests.Length && i < questsUI.Length; i++)
        {
            currentQuestToShow = quests[i];
            if(currentQuestToShow != null)
                questsUI[i].SetUp(currentQuestToShow.questName, currentQuestToShow.questDescription, currentQuestToShow.rewardWeapon != null ?currentQuestToShow.rewardWeapon.weaponName : "", currentQuestToShow.GetCurrentValue, currentQuestToShow.valueToReach, currentQuestToShow.Done);
            else
                questsUI[i].SetEmptyQuest();
        }
    }

    public void CloseQuestsFrames()
    {
        foreach (QuestFrameUI questFrame in questsUI)
            questFrame.HidePanel();
    }
}
