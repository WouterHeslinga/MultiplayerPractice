using System;
using System.Collections;
using UnityEngine;

public class TeamSpawnPosition : MonoBehaviour
{
    [SerializeField] private Team Team;
    
    private void Awake()
    {
        PlayerSpawnSystem.AddSpawnPoint(Team, transform);
    }

    private void OnDestroy()
    {
        PlayerSpawnSystem.RemoveSpawnPoint(Team, transform);
    }
}