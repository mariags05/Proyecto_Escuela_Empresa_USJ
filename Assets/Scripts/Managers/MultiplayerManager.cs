// *************************************************************** //
// Script done by Jorge Kojtych
// Centralizes the management of players
// In Progress
// Instead of a GameObject array, wee should use a PlayerController array that exposes input component when implemented
// Missing: Player disconnection, player count management, player IDs
// *************************************************************** //

using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    #region Variables
    public static MultiplayerManager Instance { get; private set; }

    [SerializeField] private int m_MaxPlayers = 4;
    private int m_PlayerCount = 0;
    private PlayerController[] m_Players;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple instances of MultiplayerManager detected. Destroying duplicate.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
    }

    private void Initialize()
    {
        m_Players = new PlayerController[m_MaxPlayers];
    }

    public void RegisterPlayer(PlayerController player)
    {
        if (m_PlayerCount < m_MaxPlayers)
        {
            m_Players[m_PlayerCount] = player;
            m_PlayerCount++;

            Debug.Log("Player registered. Total players: " + m_PlayerCount);
        }
        else
        {
            Debug.LogWarning($"Tried to add player {player.name}, but player count is already at maximum.", player);
        }
    }
}