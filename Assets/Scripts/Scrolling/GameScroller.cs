using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScroller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerBounds playerBounds = default;
    public PlayerBounds GetPlayerBounds { get { return playerBounds; } }
    [SerializeField] ChunksGenerator chunksGenerator = default;
    [SerializeField] Transform chunkEndMark = default;
    [SerializeField] float spaceBetweenTwoChunks;
    bool CanCreateANewChunk { get { return chunkEndMark.position.y < enterTransform.position.y - spaceBetweenTwoChunks && currentScrollerState == ScrollerState.Moving; } }

    PlayerSpaceShipScript player = default;
    /// <summary>
    /// If an IPullableObject
    /// </summary>
    [SerializeField] Transform killTransform = default;
    public Transform GetKillTransform { get { return killTransform; } }

    /// <summary>
    /// If an IPullableObject
    /// </summary>
    [SerializeField] Transform enterTransform = default;
    public Transform GetEnterTransform { get { return enterTransform; } }

    [Header("Scroling Balancing")]
    [SerializeField] float scrollingMaxSpeed = 2;
    [SerializeField] float scrollerAcceleration = 2;
    [SerializeField] float scrollerDesceleration = 2;
    ScrollerState currentScrollerState = ScrollerState.Stopped;
    float currentScrollingSpeed;

    [Header("Rendering")]
    [SerializeField] Transform backGroundOne = default;
    [SerializeField] Transform backGroundTwo = default;
    [SerializeField] float backgroundHeight;    
    
    public void SetUp(PlayerSpaceShipScript playerShip)
    {
        player = playerShip;
    }

    public void StartScroller()
    {
        currentScrollerState = ScrollerState.Moving;
    }

    public void StopScroller()
    {
        currentScrollerState = ScrollerState.Stopped;
    }

    public System.Action OnChunkEndReached;

    private void Update()
    {
        UpdateScrollerValues();
        UpdateScrollerPosition();

        UpdateChunksInstantiation();

        KeepPlayerInsideBounds();

        UpdateBackground();
    }

    public void UpdateScrollerValues()
    {
        switch (currentScrollerState)
        {
            case ScrollerState.Moving:
                if(currentScrollingSpeed < scrollingMaxSpeed)
                {
                    currentScrollingSpeed += scrollerAcceleration * Time.deltaTime;
                    if (currentScrollingSpeed > scrollingMaxSpeed)
                        currentScrollingSpeed = scrollingMaxSpeed;
                }
                break;

            case ScrollerState.Stopped:
                if (currentScrollingSpeed > 0)
                {
                    currentScrollingSpeed -= scrollerDesceleration * Time.deltaTime;
                    if (currentScrollingSpeed < 0)
                        currentScrollingSpeed = 0;
                }
                break;
        }
    }

    public void UpdateScrollerPosition()
    {
        transform.position += Vector3.up * currentScrollingSpeed * Time.deltaTime;
    }

    public void KeepPlayerInsideBounds()
    {
        if(player == null || playerBounds == null)
            return;

        Vector3 clampedPosition = player.transform.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, transform.position.x + (playerBounds.downLeftLimit.x), transform.position.x + (playerBounds.upRightLimit.x));
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, transform.position.y + (playerBounds.downLeftLimit.y), transform.position.y + (playerBounds.upRightLimit.y));

        player.transform.position = clampedPosition;
    }

    public void UpdateChunksInstantiation()
    {
        if (CanCreateANewChunk)
            chunksGenerator.CreateNewChunk(enterTransform.position, chunkEndMark);
    }

    public void UpdateBackground()
    {
        if(backGroundOne.transform.position.y < killTransform.position.y - backgroundHeight)
        {
            Vector3 newPos = backGroundOne.transform.position;
            newPos.y += backgroundHeight * 2;
            backGroundOne.transform.position = newPos;
        }

        if (backGroundTwo.transform.position.y < killTransform.position.y - backgroundHeight)
        {
            Vector3 newPos = backGroundTwo.transform.position;
            newPos.y += backgroundHeight * 2;
            backGroundTwo.transform.position = newPos;
        }
    }
}

public enum ScrollerState { Moving, Stopped }