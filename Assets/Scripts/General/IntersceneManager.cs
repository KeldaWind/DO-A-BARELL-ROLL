using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersceneManager : MonoBehaviour
{
    public static IntersceneManager intersceneManager;

    [SerializeField] QuestsIntersceneManagement questsIntersceneManagement;
    public QuestsIntersceneManagement GetQuestsIntersceneManagement { get { return questsIntersceneManagement; } }

    public void SetUp()
    {
        intersceneManager = this;
        DontDestroyOnLoad(gameObject);
        questsIntersceneManagement.LoadLibrary();
    }
}
