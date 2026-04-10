// *************************************************************** //
// Script done by Jorge Crespo
// Handles player´s and minigames scores.
// In progress
// TODO: Nothing for now, most likely will need to be completely refactored due to the ineptitude of the programmer.
// *************************************************************** //

using UnityEngine;

public class ScoreManager : PersistentLazySingleton<ScoreManager>
{

    // This number should ideally match multiplayer controller's maximum number of players.
    [SerializeField] private int m_MaxPlayers = 4;

    // Track the total score of each player. Ideally this is persistent.
    private int[] m_TotalPlayerScores;

    // How many points should the minigame award for each place.
    // First entry is awarded to first place, last entry is awarded to last place and so on.
    private int[] m_MinigameScoreAwards;

    // First entry is last player, last entry is first player.
    private int[] m_LosePlayerOrder;
    private int m_NextPlayerToLose;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        m_TotalPlayerScores = new int[m_MaxPlayers];
        m_MinigameScoreAwards = new int[m_MaxPlayers];
        m_LosePlayerOrder = new int[m_MaxPlayers];
        m_NextPlayerToLose = 0;
        ResetLoseOrder();
        SetMinigamePointRewards(new int[] {5, 3, 1, 0});
    }

    void Update()
    {

    }

    public int[] GetTotalPlayerScores ()
    {
        return m_TotalPlayerScores;
    }

    // Call when a player loses.
    public void AddPlayerLose(int player)
    {
        m_LosePlayerOrder[m_NextPlayerToLose] = player;
        if (m_NextPlayerToLose < m_MaxPlayers)
        {
            m_NextPlayerToLose++;
        }
    }

    public void SetMinigamePointRewards(int[] ints)
    {
        m_MinigameScoreAwards = ints;
    }

    public int[] GetMinigamePointRewards()
    {
        return m_MinigameScoreAwards;
    }

    public void ResetLoseOrder()
    {
        m_LosePlayerOrder = new int[m_MaxPlayers];
        m_NextPlayerToLose = 0;
        Debug.Log("Lose order has been reset");
    }

    // Add total score to each player when the minigame ends.
    public void AwardPointsToPlayers()
    {
        Debug.Log("Awarding points to players");
        // Goes from last place to first.
        for (int i = 0; i < m_LosePlayerOrder.Length; i++)
        {
            int m_TargetPlayer = m_LosePlayerOrder[i];
            int m_PointsToAward = m_MinigameScoreAwards[m_MaxPlayers - 1 - i];
            m_TotalPlayerScores[m_TargetPlayer] += m_PointsToAward;
            Debug.Log($"Player {m_TargetPlayer} has been awarded {m_PointsToAward} points for placing {i}");
        }
    }
}
