// *************************************************************** //
// Script done by Jorge Crespo
// Handles individual minigame's 
// In progress
// Everything
// *************************************************************** //

using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int[] m_TotalPlayerScores; // Track he total score of each player. Ideally this is persistent.
    public int[] m_MinigameScoreAwards; // How many points should the minigame award for each place.

    private int[] m_LosePlayerOrder; // First entry is last player, last entry is first player.
    void Start()
    {
        m_TotalPlayerScores = new int[4];
        m_MinigameScoreAwards = new int[4];
        ResetLoseOrder();
    }

    void Update()
    {

    }

    // Call when a player loses.
    public void AddPlayerLose(int player)
    {
        for (int i = 0; i < m_LosePlayerOrder.Length; i++)
        {
            if (m_LosePlayerOrder[i] == -1)
            {
                m_LosePlayerOrder[i] = player;
                break;
            }
        }
    }

    public void SetMinigamePointRewards(int[] ints)
    {
        m_LosePlayerOrder = ints; // You tell me what this does.
    }

    public void ResetLoseOrder()
    {
        m_LosePlayerOrder = new int[] {-1, -1, -1, -1};
    }

    // Add total score to each player when the minigame ends.
    public void AwardPointsOnMinigameEnd()
    {
        for (int i = 0; i < m_LosePlayerOrder.Length; i++)
        {
            m_TotalPlayerScores[i] += m_MinigameScoreAwards[i];
        }
    }
}
