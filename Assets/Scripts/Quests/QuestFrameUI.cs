using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestFrameUI : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup = default;
    [SerializeField] Text nameText = default;
    [SerializeField] Text descriptionText = default;
    [SerializeField] Text rewardText = default;
    [SerializeField] Text progressionText = default;
    [SerializeField] Image progressionFill = default;
    [SerializeField] Image questDoneImage = default;

    public void SetUp(string name, string description, string rewardName, float progressionValue, float targetValue, bool done)
    {
        nameText.text = name;
        descriptionText.text = description;
        progressionText.text = StaticMethodsLibrary.FloatToSingleNumberAfterComma(progressionValue) + "/" + targetValue;
        progressionFill.fillAmount = progressionValue / targetValue;

        rewardText.text = rewardName != "" && rewardName != null ? ("Reward \n" + rewardName) : "No Reward";

        questDoneImage.gameObject.SetActive(done);
    }

    public void ShowPanel()
    {
        canvasGroup.alpha = 1;
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    public void SetEmptyQuest()
    {
        canvasGroup.alpha = 0.5f;

        nameText.text = "No Quest";
        descriptionText.text = "";
        progressionText.text = "- / -";
        progressionFill.fillAmount = 0;

        rewardText.text = "";

        questDoneImage.gameObject.SetActive(false);
    }
}
