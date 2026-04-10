// *************************************************************** //
// Script done by Jorge Kojtych
// Player component that handles and exposes the input for each player
// Done?
// To create a new player controller:
// 1. Create a new script that inherits from BasePlayerController
// 2. Override the Awake method if you need to initialize any variables or components (call base.Awake() to ensure the InputController is set up)
// *************************************************************** //

using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class BasePlayerController : MonoBehaviour
{
    public PlayerInputController InputController { get; private set; }

    protected virtual void Awake()
    {
        MultiplayerManager.Instance.RegisterPlayer(this);

        InputController = GetComponent<PlayerInputController>();
        if (InputController == null)
        {
            Debug.LogWarning("Player controller has no PlayerInputController component.", this);
        }
    }

    protected virtual void OnDestroy()
    {
        // Use "InstanceExists" check to avoid instantiating a new object in OnDestroy
        if (MultiplayerManager.InstanceExists) MultiplayerManager.Instance.UnregisterPlayer(this);
    }
}