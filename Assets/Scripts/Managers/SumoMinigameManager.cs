using UnityEngine;

public class SumoMinigameManager : Singleton<SumoMinigameManager>
{
    [SerializeField] private Transform[] m_SpawnPoints;
    [SerializeField] private SumoPlayerController m_PlayerPrefab;

    private void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < MultiplayerManager.Instance.MaxPlayers; i++)
        {
            Vector2 spawnPosition = GetSpawnPosition(i);
            Instantiate(m_PlayerPrefab, spawnPosition, Quaternion.identity);
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
