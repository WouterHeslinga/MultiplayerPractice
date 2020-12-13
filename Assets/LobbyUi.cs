using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject lobbyPlayerUnitPrefab;
    private List<GameObject> unitList = new List<GameObject>();

    private GeneralNetworkManager networkManager;

    private void Awake()
    {
        networkManager = FindObjectOfType<GeneralNetworkManager>();
    }

    public void Test()
    {
        for (int i = unitList.Count - 1; i >= 0; i--)
        {
            Destroy(unitList[i]);
        }
        
        unitList.Clear();
        foreach (var player in networkManager.RoomPlayers)
        {
            var playerUnit = Instantiate(lobbyPlayerUnitPrefab, playerListParent);
            playerUnit.GetComponentInChildren<TextMeshProUGUI>().text = player.Name;
            
            unitList.Add(playerUnit);
        }
    }
}