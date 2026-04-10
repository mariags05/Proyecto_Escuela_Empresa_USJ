// *************************************************************** //
// Script done by Jorge Kojtych
// Centralizes the management of players
// In Progress
// Instead of a GameObject array, wee should use a PlayerController array that exposes input component when implemented
// Missing: Player disconnection, player count management, player IDs
// *************************************************************** //

using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : PersistentLazySingleton<MultiplayerManager>
{
    #region Variables
    [SerializeField] private int m_MaxPlayers = 4;
    private int m_PlayerCount = 0;
    private BasePlayerController[] m_Players;

    private int m_ConnectedInputDevices = 0;
    #endregion

    public int MaxPlayers => m_MaxPlayers;
    public int PlayerCount => m_PlayerCount;
    public int ConnectedInputDevices => m_ConnectedInputDevices;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void Initialize()
    {
        m_Players = new BasePlayerController[m_MaxPlayers];

        // Count initial connected devices
        m_ConnectedInputDevices = Gamepad.all.Count;
        bool hasKeyboard = Keyboard.current != null;
        bool hasMouse = Mouse.current != null;
        if (hasKeyboard || hasMouse) m_ConnectedInputDevices++;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                m_ConnectedInputDevices++;
                break;
            case InputDeviceChange.Removed:
                m_ConnectedInputDevices--;
                break;
        }
    }

    public void RegisterPlayer(BasePlayerController player)
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

    public void UnregisterPlayer(BasePlayerController player)
    {
        for (int i = 0; i < m_PlayerCount; i++)
        {
            if (m_Players[i] == player)
            {
                // Shift remaining players down to fill the gap
                for (int j = i; j < m_PlayerCount - 1; j++)
                {
                    m_Players[j] = m_Players[j + 1];
                }
                m_Players[m_PlayerCount - 1] = null;
                m_PlayerCount--;

                Debug.Log("Player unregistered. Total players: " + m_PlayerCount);
                return;
            }
        }

        Debug.LogWarning($"Tried to remove player {player.name}, but they were not found in the player list.", player);
    }
}