using UnityEngine;

public class SumoHitboxScript : MonoBehaviour
{
    [SerializeField] private float m_KnockbackForce = 10f;
    private Collider2D m_Collider;

    private void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        // Ignore collisions with the ring
        if (SumoMinigameManager.Instance != null && SumoMinigameManager.Instance.RingGameObject != null)
        {
            Collider2D hitboxCollider = m_Collider;
            Collider2D ringCollider = SumoMinigameManager.Instance.RingCollider;

            if (hitboxCollider != null && ringCollider != null)
            {
                Physics2D.IgnoreCollision(hitboxCollider, ringCollider);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject != SumoMinigameManager.Instance.RingGameObject)
        {
            if (collision.TryGetComponent(out SumoPlayerController player))
            {
                // Apply a force to the player away from the center of the hitbox
                Vector2 direction = (player.transform.position - transform.parent.position).normalized;
                player.Rigidbody.linearVelocity = Vector2.zero;
                player.ApplyKnockback(direction, m_KnockbackForce);
            }
        }
    }
}