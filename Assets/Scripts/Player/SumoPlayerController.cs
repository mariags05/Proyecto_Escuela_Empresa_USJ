using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SumoPlayerController : BasePlayerController
{
    private Rigidbody2D m_Rigidbody;
    [SerializeField] private float m_MoveForce = 10f;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        if (m_Rigidbody == null)
        {
            Debug.LogWarning("SumoPlayerController has no Rigidbody component.", this);
        }
    }

    private void FixedUpdate()
    {
        if (InputController == null || m_Rigidbody == null) return;

        Vector2 input = InputController.MoveInput;
        m_Rigidbody.AddForce(input * m_MoveForce);
    }
}
