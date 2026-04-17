// *************************************************************** //
// Script done by Jorge Kojtych
// Singleton manager for the Sumo minigame, responsible for spawning players and managing game state
// In progress
// *************************************************************** //

using UnityEngine;

public class SumoMinigameManager : BaseMinigameManager<SumoMinigameManager, SumoPlayerController>
{
    [field: SerializeField] public GameObject RingGameObject { get; private set; }
    public Collider2D RingCollider { get; private set; }

    protected override void Start()
    {
        base.Start();
        RingCollider = RingGameObject.GetComponent<Collider2D>();
    }
}
