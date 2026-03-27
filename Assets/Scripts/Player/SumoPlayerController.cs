// *************************************************************** //
// Script done by Jorge Kojtych
// Player component that handles player control for the Sumo minigame
// In progress
// *************************************************************** //

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SumoPlayerController : BasePlayerController
{
    private Rigidbody2D m_Rigidbody;
    [SerializeField] private float m_MoveForce = 10f;
    [SerializeField] private float m_RotationSpeed = 100f;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        if (m_Rigidbody == null)
        {
            Debug.LogWarning("SumoPlayerController has no Rigidbody component.", this);
        }
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
        if (InputController == null || m_Rigidbody == null) return;

        Vector2 input = InputController.MoveInput;
        m_Rigidbody.AddForce(input * m_MoveForce);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == SumoMinigameManager.Instance.RingGameObject)
        {
            StartCoroutine(DieCoroutine(1f));
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

        Destroy(gameObject);
    }
}
