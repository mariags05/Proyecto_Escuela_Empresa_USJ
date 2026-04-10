// *************************************************************** //
// Script done by Jorge Kojtych
// Singleton base manager for minigames, responsible for spawning players and managing game state
// To create a new minigame manager:
// 1. Create a new script that inherits from BaseMinigameManager<TPlayer>, where TPlayer is the specific player controller for that minigame
// 2. Call base.Start() in the Start method if you override it
// 3. Add "public new static YourMinigameManager Instance => BaseMinigameManager<YourPlayerController>.Instance as YourMinigameManager;"
// If in doubt, look at SumoMinigameManager for an example
// Done?
// *************************************************************** //

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class BaseMinigameManager<TPlayer> : Singleton<BaseMinigameManager<TPlayer>> where TPlayer : BasePlayerController
{
    [SerializeField] private Transform[] m_SpawnPoints;
    [SerializeField] private TPlayer m_PlayerPrefab;

    // Lists to keep track of spawned players
    private List<TPlayer> m_AlivePlayers = new List<TPlayer>(4);
    private List<TPlayer> m_DeadPlayers = new List<TPlayer>(4);

    public UnityEvent<IReadOnlyList<TPlayer>> OnGameOver;

    protected virtual void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        int playersToSpawn = MultiplayerManager.Instance.ConnectedInputDevices;

        for (int i = 0; i < playersToSpawn; i++)
        {
            Vector2 spawnPosition = GetSpawnPosition(i);

            var player = Instantiate(m_PlayerPrefab, spawnPosition, Quaternion.identity);
            player.name = $"{m_PlayerPrefab.name} {i + 1}";
            player.OnDeath.AddListener(specificPlayerController => HandlePlayerDeath(specificPlayerController as TPlayer));
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
            Debug.LogWarning("No spawn points assigned to SumoMinigameManager.");
            return Vector2.zero;
        }

        // In case there are not enough spawn points, loop back
        int index = playerIndex % m_SpawnPoints.Length;
        return m_SpawnPoints[index].position;
    }

    private void HandlePlayerDeath(TPlayer player)
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
        if (m_AlivePlayers.Count == 1)
        {
            Debug.Log($"Player {m_AlivePlayers[0].name} wins!");

            // Create a new list with all players in order (first is the winner, last is the first to die)
            List<TPlayer> orderedPlayers = new List<TPlayer>(m_AlivePlayers);
            for (int i = m_DeadPlayers.Count - 1; i >= 0; i--)
            {
                orderedPlayers.Add(m_DeadPlayers[i]);
            }

            OnGameOver?.Invoke(orderedPlayers);
        }
    }
}