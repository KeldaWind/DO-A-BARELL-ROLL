using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [Header("Important References")]
    [SerializeField] PlayerSpaceShipScript player = default;
    public PlayerSpaceShipScript GetPlayer { get { return player; } }

    [SerializeField] Camera mainCamera = default;
    public Camera GetMainCamera { get { return mainCamera; } }

    [SerializeField] GameScroller gameScroller = default;
    public GameScroller GetGameScroller { get { return gameScroller; } }

    private void Awake()
    {
        gameManager = this;

        if (gameScroller != null)
            gameScroller.SetUp(player);
    }
}
