// *************************************************************** //
// Script done by Jorge Kojtych
// Singleton manager for the Sumo minigame, responsible for spawning players and managing game state
// In progress
// *************************************************************** //

using UnityEngine;

public class SumoMinigameManager : Singleton<SumoMinigameManager>
{
    [SerializeField] private Transform[] m_SpawnPoints;
    [SerializeField] private SumoPlayerController m_PlayerPrefab;

    [field: SerializeField] public GameObject RingGameObject { get; private set; }

    private void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        int playersToSpawn = MultiplayerManager.Instance.ConnectedInputDevices;

        for (int i = 0; i < playersToSpawn; i++)
        {
            Vector2 spawnPosition = GetSpawnPosition(i);
            Instantiate(m_PlayerPrefab, spawnPosition, Quaternion.identity);
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
}
