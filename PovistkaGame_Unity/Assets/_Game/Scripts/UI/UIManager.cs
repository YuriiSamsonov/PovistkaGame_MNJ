using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Systems.Player;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [field: SerializeField] private GameObject fToShowWaypointerScreen;
    
    [field: SerializeField] private GameObject playerIsDeadScreen;

    [field: SerializeField] private GameObject youNeedAKeyScreen;

    [field: SerializeField] private GameObject eToTakeTheKeyScereen;
    
    private IEnumerator ShowWaypointerForSeconds()
    {
        yield return new WaitForSecondsRealtime(3);
        fToShowWaypointerScreen.SetActive(false);
    }
    private void FToShowWaypointerEventHandler(EventArgs _)
    {
        fToShowWaypointerScreen.SetActive(true);
        StartCoroutine(ShowWaypointerForSeconds());
    }

    private void OnPlayerHasDiedEventHandler(EventArgs _)
    {
        playerIsDeadScreen.SetActive(true);
    }

    private void OnPlayerHasDiedHandlerWithRoutine(EventArgs _)
    {
        StartCoroutine(PlayerDiedWithThreeSeconds());
    }

    private IEnumerator PlayerDiedWithThreeSeconds()
    {
        yield return new WaitForSecondsRealtime(6);
        playerIsDeadScreen.SetActive(true);
    }

    private void OnPlayerNearTheKey(InteractionEventArgs args)
    {
        eToTakeTheKeyScereen.SetActive(args.State);
    }

    private void OnPlayerNearClsoedDoor(InteractionEventArgs args)
    {
        youNeedAKeyScreen.SetActive(args.State);
    }

    public void SubscribeToEvents(Player player, EnemyManager enemyManager)
    {
        player.StartLevelEvent += FToShowWaypointerEventHandler;
        player.PlayerNearTheKeyEvent += OnPlayerNearTheKey;
        player.PlayerNearClosedDoorEvent += OnPlayerNearClsoedDoor;
        player.CutsceneEvent += OnPlayerHasDiedHandlerWithRoutine;
        enemyManager.AttemptToLillPlayerEvent += OnPlayerHasDiedEventHandler;
    }
}
