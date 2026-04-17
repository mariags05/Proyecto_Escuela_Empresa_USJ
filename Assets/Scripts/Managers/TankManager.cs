using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class TankManager : MonoBehaviour
{
    //*************************************************************************************************
    // Script done by Jorge Kojtych & Jorge Cristobal?
    // Script done for managing multiple players on the tank minigame

    // State of the script [IN PROGRESS]

    //*************************************************************************************************             
    //public GameObject t_TankPlayer;
    [SerializeField] private Transform[] m_SpawnPoints;
    [SerializeField] private TankPlayerControler m_PlayerPrefab;

    public int t_MaxPlayers = 4;
    public int t_MinPlayers = 1;
    public int t_currentPlayers;

    private List<TankPlayerControler> m_AlivePlayers = new List<TankPlayerControler>(4);
    private List<TankPlayerControler> m_DeadPlayers = new List<TankPlayerControler>(4);

    // public UnityEvent<IReadOnlyList<TankPlayerControler>> OnGameOver;

    // public List<GameObject> t_SpawnPoints = new List<GameObject>();

    private void Awake()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        if (m_PlayerPrefab == null)
        {
            Debug.LogError("TankManager: m_PlayerPrefab is not assigned on inspector. Please assing the prefab of the player.");
            return;
        }
        int playersToSpawn = MultiplayerManager.Instance.ConnectedInputDevices;

        for (int i = 0; i < playersToSpawn; i++)
        {
            Vector2 spawnPosition = GetSpawnPosition(i);

            var player = Instantiate(m_PlayerPrefab, spawnPosition, Quaternion.identity);
            player.name = $"{m_PlayerPrefab.name} {i + 1}";
            //player.OnDeath.AddListener(HandlePlayerDeath);
            m_AlivePlayers.Add(player);
        }

        if (playersToSpawn < MultiplayerManager.Instance.MaxPlayers)
        {
            Debug.LogWarning($"Only {playersToSpawn} player(s) will be active due to the number of connected input devices.");
        }
    }

    private Vector2 GetSpawnPosition(int playerIndex)
    {
        if (m_SpawnPoints == null || m_SpawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned to TankMiniGame.");
            return Vector2.zero;
        }

        // In case there are not enough spawn points, loop back
        int index = playerIndex % m_SpawnPoints.Length;
        return m_SpawnPoints[index].position;
    }
    private void HandlePlayerDeath(TankPlayerControler player)
    {
        if (m_AlivePlayers.Contains(player))
        {
            m_AlivePlayers.Remove(player);
            m_DeadPlayers.Add(player);

            CheckForGameOver();
        }
    }

    private void CheckForGameOver()
    {
        /*if (m_AlivePlayers.Count == 1)
        {
            Debug.Log($"Player {m_AlivePlayers[0].name} wins!");

            // Create a new list with all players in order (first is the winner, last is the first to die)
            List<TankPlayerControler> orderedPlayers = new List<TankPlayerControler>(m_AlivePlayers);
            for (int i = m_DeadPlayers.Count - 1; i >= 0; i--)
            {
                orderedPlayers.Add(m_DeadPlayers[i]);
            }

            OnGameOver?.Invoke(orderedPlayers);
        }*/
    }

}
