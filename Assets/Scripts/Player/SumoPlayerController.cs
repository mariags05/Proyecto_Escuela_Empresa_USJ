// *************************************************************** //
// Script done by Jorge Kojtych
// Player component that handles player control for the Sumo minigame
// In progress
// *************************************************************** //

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class SumoPlayerController : BasePlayerController
{
    public Rigidbody2D Rigidbody { get; private set; }

    [SerializeField] private float m_MoveForce = 10f;
    [SerializeField] private float m_RotationSpeed = 100f;

    [SerializeField] private GameObject m_AttackHitbox;

    public UnityEvent<SumoPlayerController> OnDeath;

    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();
        Rigidbody = GetComponent<Rigidbody2D>();
        if (Rigidbody == null)
        {
            Debug.LogWarning("SumoPlayerController has no Rigidbody component.", this);
        }

        if (m_AttackHitbox != null) m_AttackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (InputController == null) return;

        Vector2 moveInput = InputController.MoveInput;
        if (moveInput.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.forward, moveInput),
                Time.deltaTime * m_RotationSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (InputController == null || Rigidbody == null) return;

        Vector2 input = InputController.MoveInput;
        Rigidbody.AddForce(input * m_MoveForce);
    }

    private void OnEnable()
    {
        if (InputController != null)
        {
            InputController.OnPrimaryAction.AddListener(OnPrimaryAction);
        }
    }

    private void OnDisable()
    {
        if (InputController != null)
        {
            InputController.OnPrimaryAction.RemoveListener(OnPrimaryAction);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return;
        if (collision.gameObject == SumoMinigameManager.Instance.RingGameObject)
        {
            StartCoroutine(DieCoroutine(1f));
        }
    }
    #endregion

    #region Action Handlers
    private void OnPrimaryAction(bool isPressed)
    {
        if (m_AttackHitbox != null && isPressed && !m_AttackHitbox.activeSelf)
        {
            m_AttackHitbox.SetActive(true);
            StartCoroutine(DisableHitboxAfterDelay(0.5f));
        }
    }
    #endregion

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (Rigidbody != null)
        {
            Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    private IEnumerator DieCoroutine(float time)
    {
        float timer = 0f;
        Vector3 originalScale = transform.localScale;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, timer / time);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        OnDeath?.Invoke(this);

        Destroy(gameObject);
    }

    private IEnumerator DisableHitboxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (m_AttackHitbox != null) m_AttackHitbox.SetActive(false);
    }
}
