using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    // Script done by [Jorge Cristobal]
    // Scrip [TERMINADO]

    // Script hecho para controlar el comportamiento del proyectil
    // Bugs encontrados:
    // - EL tanque se autodestruye a si mismo 
    // - La velocidades se mantienen al dejarse de mover el tanke

    public UnityEvent p_shootingDeacttivationEvent = new UnityEvent();

    [SerializeField] private int p_MaxCollisions = 3;
    [SerializeField] private float p_MinSpeedToLive = 0.5f;
    [SerializeField] private float p_SpeedReductionCoeficient = 0.45f;

    private int p_CurrentCollisions = 0;
    [SerializeField] private Rigidbody p_Rb;

    private void Awake()
    {
        p_Rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (p_Rb.linearVelocity.magnitude < p_MinSpeedToLive || p_CurrentCollisions > 2)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void ResetCollisions()
    {
        p_CurrentCollisions = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Impacta al jugador: destruye el jugador y devuelve el proyectil al pool
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
            Debug.Log("Papaya");
            return;
        }
        else
        {
            // Rebote: reduce velocidad y suma colisión
            p_Rb.linearVelocity *= p_SpeedReductionCoeficient;
            p_CurrentCollisions++;


        }
    }
    private void OnDisable()
    {
        // Guard in case no listeners are attached
        p_shootingDeacttivationEvent?.Invoke();
    }
}