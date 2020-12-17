using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipAdress;

    private GeneralNetworkManager networkManager;

    private void Awake()
    {
        networkManager = FindObjectOfType<GeneralNetworkManager>();
    }

    public void JoinServer()
    {
        networkManager.networkAddress = ipAdress.text == "" ? "localhost" : ipAdress.text;
        networkManager.StartClient();
    }

    public void HostServer()
    {
        networkManager.StartHost();
    }
}