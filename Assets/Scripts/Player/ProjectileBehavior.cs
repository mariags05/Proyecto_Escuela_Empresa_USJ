using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    // Script done by [Jorge Cristobal]
    // Scrip [TERMINADO]

    // Script hecho para controlar el comportamiento del proyectil

    [SerializeField] private int p_MaxCollisions = 3;
    [SerializeField] private float p_MinSpeedToLive = 0.5f;

    private int p_CurrentCollisions = 0;
    private Rigidbody p_Rb;

    private void Awake()
    {
        p_Rb = GetComponent<Rigidbody>();
    }

    public void ResetCollisions()
    {
        p_CurrentCollisions = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Impacta al jugador: destruye el jugador y devuelve el proyectil al pool
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            // Rebote: reduce velocidad y suma colisión
            p_Rb.linearVelocity *= 0.66f;
            p_CurrentCollisions++;

            // Desactiva si superó rebotes o va demasiado lento
            if (p_CurrentCollisions >= p_MaxCollisions || p_Rb.linearVelocity.magnitude < p_MinSpeedToLive)
            {
                gameObject.SetActive(false);
            }
        }
    }
    
}