// *************************************************************** //
// Script done by Jorge Kojtych
// Singleton manager for the Sumo minigame, responsible for spawning players and managing game state
// In progress
// *************************************************************** //

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class SumoMinigameManager : Singleton<SumoMinigameManager>
{
    [SerializeField] private Transform[] m_SpawnPoints;
    [SerializeField] private SumoPlayerController m_PlayerPrefab;

    [field: SerializeField] public GameObject RingGameObject { get; private set; }
    public Collider2D RingCollider { get; private set; }

    // Lists to keep track of spawned players
    private List<SumoPlayerController> m_AlivePlayers = new List<SumoPlayerController>(4);
    private List<SumoPlayerController> m_DeadPlayers = new List<SumoPlayerController>(4);

    public UnityEvent<IReadOnlyList<SumoPlayerController>> OnGameOver;

    private void Start()
    {
        RingCollider = RingGameObject.GetComponent<Collider2D>();
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
            player.OnDeath.AddListener(HandlePlayerDeath);
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

    private void HandlePlayerDeath(SumoPlayerController player)
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
            List<SumoPlayerController> orderedPlayers = new List<SumoPlayerController>(m_AlivePlayers);
            for (int i = m_DeadPlayers.Count - 1; i >= 0; i--)
            {
                orderedPlayers.Add(m_DeadPlayers[i]);
            }

            OnGameOver?.Invoke(orderedPlayers);
        }
    }
}
