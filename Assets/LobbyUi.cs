using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject lobbyPlayerUnitPrefab;
    private List<GameObject> unitList = new List<GameObject>();

    private GeneralNetworkManager networkManager;
    private RoomPlayer roomPlayer;

    private void Awake()
    {
        networkManager = FindObjectOfType<GeneralNetworkManager>();
    }

    public void UpdatePlayerList()
    {
        for (int i = unitList.Count - 1; i >= 0; i--)
        {
            Destroy(unitList[i]);
        }
        
        unitList.Clear();
        foreach (var player in networkManager.RoomPlayers)
        {
            var playerUnit = Instantiate(lobbyPlayerUnitPrefab, playerListParent);
            playerUnit.GetComponentInChildren<TextMeshProUGUI>().text = $"{player.Name} {(player.IsReady ? "Ready": "Not Ready")}";
            
            unitList.Add(playerUnit);
        }
    }

    public void Initialize(RoomPlayer player)
    {
        roomPlayer = player;

        RoomPlayer.OnPlayerChanged += UpdatePlayerList;
    }

    public void ToggleReady()
    {
        roomPlayer.CmdSetReadyState(!roomPlayer.IsReady);
    }

    public void StartGame()
    {
        roomPlayer.CmdStartGame();
    }
}