// *************************************************************** //
// Script done by Jorge Crespo
// Meant to test the score manager's functions
// Temporary until a better nethod is found.
// *************************************************************** //

using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreManagerTester : MonoBehaviour
{
    ScoreManager sc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sc = GetComponent<ScoreManager>();

        sc.SetMinigamePointRewards(new int[] {4, 3, 2, 1});
        sc.AddPlayerLose(0);
        sc.AddPlayerLose(2);
        sc.AddPlayerLose(3);
        sc.AddPlayerLose(1);
        sc.AwardPointsToPlayers();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
