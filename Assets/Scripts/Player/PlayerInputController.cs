// *************************************************************** //
// Script done by Jorge Kojtych
// Player component that handles and exposes the input for each player
// In progress
// To add more actions, add more InputActionReferences and corresponding callback methods in OnEnable and OnDisable
// *************************************************************** //

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    public PlayerInput PlayerInputComponent { get; private set; }
    public int PlayerID => PlayerInputComponent.playerIndex;

    #region Input Actions
    [Header("Input Action References")]
    [SerializeField] private InputActionReference m_MoveActionRef;
    public InputAction MoveAction { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public UnityEvent<Vector2> OnMoveInput;

    // Add more InputActionReferences and InputActions
    #endregion

    #region Unity Methods
    private void Awake()
    {
        PlayerInputComponent = GetComponent<PlayerInput>();

        MoveAction = GetPlayerActionByRef(m_MoveActionRef);
        // Add more actions here as needed

        foreach (var device in PlayerInputComponent.devices)
        {
            Debug.Log($"Player {PlayerID} is using device: {device.displayName}", this);
        }
    }

    private void OnEnable()
    {
        SubscribeToAction(MoveAction, OnMovePerformed, OnMoveCanceled);
        // Subscribe to more actions here
    }

    private void OnDisable()
    {
        UnsubscribeFromAction(MoveAction, OnMovePerformed, OnMoveCanceled);
        // Unsubscribe from more actions here
    }
    #endregion

    #region Input Callbacks
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        MoveInput = moveInput;
        OnMoveInput.Invoke(moveInput);
        Debug.Log($"Player {PlayerID} Move Input: {moveInput}", this);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        MoveInput = Vector2.zero;
        OnMoveInput.Invoke(Vector2.zero);
        Debug.Log($"Player {PlayerID} Move Input: Canceled", this);
    }
    #endregion

    #region Input Action Helpers
    private void SubscribeToAction(InputAction action, Action<InputAction.CallbackContext> onPerformed, Action<InputAction.CallbackContext> onCanceled)
    {
        if (action != null)
        {
            action.performed += onPerformed;
            action.canceled += onCanceled;
        }
    }

    private void UnsubscribeFromAction(InputAction action, Action<InputAction.CallbackContext> onPerformed, Action<InputAction.CallbackContext> onCanceled)
    {
        if (action != null)
        {
            action.performed -= onPerformed;
            action.canceled -= onCanceled;
        }
    }

    private InputAction GetPlayerActionByRef(InputActionReference actionRef)
    {
        if (actionRef == null || actionRef.action == null)
        {
            Debug.LogWarning($"InputActionReference is null on {gameObject.name}", this);
            return null;
        }

        return GetPlayerActionByID(actionRef.action.id);
    }

    // Ensures input is active and finds the action in this player's aciton map
    private InputAction GetPlayerActionByID(Guid actionId)
    {
        EnsureInputActive();

        InputAction playerAction = PlayerInputComponent.actions.FindAction(actionId.ToString());
        if (playerAction == null)
        {
            Debug.LogWarning($"Could not find action with ID '{actionId}' in player {PlayerID}'s actions (Is the correct action map selected?)", this);
            return null;
        }

        if (!playerAction.enabled) playerAction.Enable();
        return playerAction;
    }

    private void EnsureInputActive()
    {
        if (!PlayerInputComponent.inputIsActive)
        {
            PlayerInputComponent.ActivateInput();
        }
    }
    #endregion
}
