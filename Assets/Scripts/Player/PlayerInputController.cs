// *************************************************************** //
// Script done by Jorge Kojtych
// Player component that handles and exposes the input for each player
// In progress
// To add more actions, add more InputActionReferences and call the TryEnableAction and TryDisableAction functions in the corresponding functions
// *************************************************************** //

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    public PlayerInput PlayerInputComponent { get; private set; }
    [SerializeField] private int m_PlayerID;

    #region Input Actions
    [SerializeField] private InputActionReference m_MoveAction;
    public Vector2 MoveInput { get; private set; }
    public UnityEvent<Vector2> OnMoveInput;

    // Add more InputActionReferences as needed for different actions
    #endregion

    private void Awake()
    {
        PlayerInputComponent = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        TryEnableAction(m_MoveAction);
    }

    private void OnDisable()
    {
        TryDisableAction(m_MoveAction);
    }

    private void Update()
    {
        // MOVE
        if (IsActionValidAndEnabled(m_MoveAction))
        {
            Vector2 moveInput = m_MoveAction.action.ReadValue<Vector2>();
            MoveInput = moveInput;
            OnMoveInput.Invoke(moveInput);
        }
    }

    #region Private Helper Methods for Input Actions
    private void TryEnableAction(InputActionReference actionRef)
    {
        if (IsActionValid(actionRef) && !actionRef.action.enabled)
        {
            actionRef.action.Enable();
        }
        else if (!IsActionValid(actionRef))
        {
            Debug.LogWarning($"Input Action Reference {actionRef} is not valid on {gameObject.name}");
        }
    }

    private void TryDisableAction(InputActionReference actionRef)
    {
        if (IsActionValidAndEnabled(actionRef))
        {
            actionRef.action.Disable();
        }
    }

    private bool IsActionValid(InputActionReference actionRef)
    {
        return actionRef != null && actionRef.action != null;
    }

    private bool IsActionValidAndEnabled(InputActionReference actionRef)
    {
        return IsActionValid(actionRef) && actionRef.action.enabled;
    }
    #endregion
}
