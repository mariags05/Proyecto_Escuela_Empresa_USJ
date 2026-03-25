using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class TankPlayerControler: MonoBehaviour
{ 
    //Script done by [Jorge Cristobal]
    //Script for managin the player movement and shoot actions

    /* State of the Script [ENDED]*/

    [SerializeField] Rigidbody m_TankRb1;
    [SerializeField] Rigidbody m_Proyectile;
    [SerializeField] private Transform m_CannonTip;

    private float m_CurrentSpeed;
    private float m_MaxSpeed;
                     
    private float m_CurrentRotationalSpeed;
    private float m_MaxRotationalSpeed;
                 

    Transform[] m_Proyectil;

    [SerializeField] private float m_CoolDownShoot = 0.9f;
    private float m_ShootTimer = 0f;




    void Start()
    {
        m_TankRb1 = GetComponent<Rigidbody>();
    }

   
    void Update()
    {
        float dt = Time.deltaTime;
        Movement(dt);
        ShootingAction(dt);
    }
   
    //Movimiento del tanque
    private void Movement(float dt)
    {
        //Se obtienen los inputs
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        

        //Aplicamos velocidad solo cuando pulsamos los correspondientes botones (verticales), dejando el otro eje con la velocidad que lleva
        if (Mathf.Abs(xInput) >= 0)
        {
            float rotationAmount = -xInput * m_MaxRotationalSpeed * dt;
            float rotation = -xInput * m_MaxRotationalSpeed * dt;
            Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);
        }

        // Vertical: mueve el tanque en la dirección en que apunta 
        // transform.up rota automáticamente con el tanque y con ello siempre apunta donde mira
        m_TankRb1.linearVelocity = transform.up * (yInput * m_MaxSpeed);
    }

    //Funcion para disparar una proyectil
    private void ShootingAction(float dt)
    {
        m_ShootTimer -= dt;

        if (Input.GetKeyDown(KeyCode.Space) && m_ShootTimer <= 0f)
        {
            GameObject projectile = ProjectileScript.Instance.RequestProyectile();

            if (projectile != null)
            {
                // Spawn en la punta del cañón; si no hay CannonTip, usa el centro del tanque
                Vector2 spawnPos = m_CannonTip != null ? m_CannonTip.position : transform.position;

                projectile.transform.position = spawnPos;
                projectile.transform.rotation = transform.rotation;

                // Lanza el proyectil en la dirección en que mira el tanque (transform.up)
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null) { rb.linearVelocity = transform.up * ProjectileScript.Instance.p_ProjectileSpeed; }

                m_ShootTimer = m_CoolDownShoot;
            }
        }
    }

    //Gestion de la colision de los proyectiles y colisiones de estos con el entorno y enemigos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            Destroy(gameObject);
        }
    }
}
