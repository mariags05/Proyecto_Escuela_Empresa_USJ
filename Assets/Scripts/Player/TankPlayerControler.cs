using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

public class TankPlayerControler: MonoBehaviour
{ 
    //Script done by [Jorge Cristobal]
    //Script for managin the player movement and shoot actions

    /* State of the Script [IN PROGRESS]*/

    // Remaining things to do:
    // Direccion de apuntado == direccion en la que se mueve (y rota) el jugador
    // CollisionEnter para los proyectiles que se eliminen a los 3 rebotes
    // 1 rebote == muerte o bajar vida en base a daño que haga por distancia
    // si velocidad del proyectil bajo -> proyectil desaparece


    [SerializeField] Rigidbody m_TankRb1;
    [SerializeField] Rigidbody m_Proyectile;

    private float m_CurrentSpeed;
    private float m_MaxSpeed;
                 
    private float m_CurrentAcceleration;
    private float m_MaxAcceleration;
                  
    private float m_CurrentRotationalSpeed;
    private float m_MaxRotationalAcceleration;
                 
    private float m_CurrentDeceleration;
    private float m_MaxDeceleration;


    // A los 3 rebotes la pelota desaparece
    //Usar mejor una lista autogenerada por los disparos?
    Transform[] m_Proyectil; 

    private float m_CoolDownShoot = 0.9f;
    private float m_ProyectilMaxAcceleration;
    private float m_ProyectilMaxDeceleration;
    private float m_ProyectilAcceleration;
    private float m_ProyectilDeceleration;
    private int m_MaxCollisions = 3;
    private int m_CurrentCollision = 0;
    private bool m_temporizadorActivo = false;



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
    //FIxedUpdate puesto para manejar las fisicas del rebote de partículas.
    private void FixedUpdate()
    {
        //OnCollisionEnter2D();
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
            m_TankRb1.linearVelocity = new Vector2(xInput * m_CurrentSpeed * dt, m_TankRb1.linearVelocity.y);
        }

        if (Mathf.Abs(yInput) >= 0)
        {
            //Dado que estamos en 3D y se pretende hacer un juego 2D tengo que usar diferentes metodos de rotación?
            m_CurrentRotationalSpeed += (m_CurrentSpeed + m_CurrentAcceleration) * dt;
            m_TankRb1.linearVelocity = new Vector3(m_TankRb1.linearVelocity.x, 0, yInput * m_CurrentRotationalSpeed);
        }
       
    }

    //Funcion para disparar una proyectil
    private void ShootingAction(float dt)
    {
        
        //3º Asigna cooldown de disparo

        if (m_temporizadorActivo)
        {
            //Verifica si sigue en cooldown
            if (m_CoolDownShoot > 0)
            {
                m_CoolDownShoot -= dt;
            }
            // Si el cooldown es menor volvemos a introducir los valores originales
            else
            {
                m_CoolDownShoot = 0.6f;
                m_temporizadorActivo = false;
            }
          
        }
        //1º obtenemos los controles de disparo
        else if (Input.GetKeyDown("Space"))
        {
            //2º Asignamos velocidad velocidad al proyectil
            m_temporizadorActivo = true;
            m_Proyectile.linearVelocity = new Vector2(m_ProyectilAcceleration, 0);
        }
    }

    //Gestion de la colision de los proyectiles y colisiones de estos con el entorno y enemigos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si colisiona con otro proyectil se destruye el tanque. 
        //¿Y si el jugador avanza a la par que dispara, podria darse a si mismo y suicidarse? ¿Como evitamos el suicidio por medio del propio proyectil?
        //Añadimos retroceso al disparar? Osea, sumamos una deceleración a la actual velocidad
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            Destroy(m_TankRb1);
        }
        else
        {
            // m_CurrentCollision++;
            if(m_CurrentCollision == 3)
            {
                Destroy(m_TankRb1);
            }
        }
    }
}
