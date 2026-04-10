// *************************************************************** //
// Script done by Jorge Kojtych
// Singleton manager for the Sumo minigame, responsible for spawning players and managing game state
// In progress
// *************************************************************** //

using UnityEngine;

public class SumoMinigameManager : BaseMinigameManager<SumoPlayerController>
{
    [field: SerializeField] public GameObject RingGameObject { get; private set; }
    public Collider2D RingCollider { get; private set; }

    public new static SumoMinigameManager Instance => BaseMinigameManager<SumoPlayerController>.Instance as SumoMinigameManager;

    protected override void Start()
    {
        base.Start();
        RingCollider = RingGameObject.GetComponent<Collider2D>();
    }
}
