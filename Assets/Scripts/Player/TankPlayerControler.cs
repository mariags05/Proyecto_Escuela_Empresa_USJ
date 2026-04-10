using UnityEngine;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class TankPlayerControler: MonoBehaviour
{ 
    //Script done by [Jorge Cristobal]
    //Script for managin the player movement and shoot actions

    /* State of the Script [ENDED]*/

    [SerializeField] Rigidbody m_TankRb1;
    [SerializeField] Rigidbody m_Proyectile;
    [SerializeField] private Transform m_CannonTip;
    public PlayerInput m_PlayerInput;
    private Vector2 m_Moving;

    private float m_CurrentSpeed;
    private float m_MaxSpeed= 10f;
                     
    private float m_CurrentRotationalSpeed;
    private float m_MaxRotationalSpeed = 2f;

    [SerializeField] private float m_Acceleration = 20f;       
    [SerializeField] private float m_AngularAcceleration = 20f; 
    [SerializeField] private float m_LinearDamping = 10f;       
    [SerializeField] private float m_AngularDamping = 10f;

    [SerializeField] private float m_linearDecceleration = -4f;
    [SerializeField] private float m_angularDecceleration = -8f;


    Transform[] m_Proyectil;

    [SerializeField] private float m_CoolDownShoot = 0.9f;
    [SerializeField] private bool m_isShooting = false;
    private float m_ShootTimer = 0.5f;

    private ProjectileBehavior m_projectile;

    void Start()
    {
        m_TankRb1 = GetComponent<Rigidbody>();
        
        m_PlayerInput.SwitchCurrentActionMap("Tank");
    }

   
    void Update()
    {
        //InputAction.CallbackContext action;
       // Movement(action);
    }

    // Solo almacena el valor; no aplica física aquí
    public void Movement(InputAction.CallbackContext action)
    {
        m_Moving = action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // ROTACIÓN: eje horizontal (m_Moving.x) -> AddTorque sobre transform.up
       /* m_TankRb1.AddTorque(
            m_Moving.x * m_MaxRotationalSpeed * transform.up * m_angularDecceleration,
            ForceMode.Acceleration
        );*/

        // MOVIMIENTO: eje vertical (m_Moving.y) -> AddForce en la dirección que mira el tanque
         m_TankRb1.AddForce(
             transform.up * -m_Moving.y * m_MaxSpeed * m_linearDecceleration,
             ForceMode.Acceleration 
         );
        m_TankRb1.transform.Rotate(transform.forward, m_AngularDamping * Time.deltaTime * -m_Moving.x);
    }

   
    //Funcion para disparar una proyectil
    public void ShootingAction(InputAction.CallbackContext action)
    {
        //m_ShootTimer -= dt;

        //if (Input.GetKeyDown(KeyCode.Space) && m_ShootTimer <= 0f)
        //{
        if (m_isShooting) 
        {
            return;
        }
            GameObject projectile = ProjectileScript.Instance.RequestProyectile();

            if (projectile != null)
            {
                // Spawn en la punta del cañón; si no hay CannonTip, usa el centro del tanque
                Vector2 spawnPos = m_CannonTip != null ? m_CannonTip.position  : transform.position;

                projectile.transform.position = spawnPos;
                projectile.transform.rotation = transform.rotation;

                // Lanza el proyectil en la dirección en que mira el tanque (transform.up)
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null) { rb.linearVelocity = transform.up * ProjectileScript.Instance.p_ProjectileSpeed; }
                m_ShootTimer = m_CoolDownShoot;
                m_isShooting = true;
                // Get the ProjectileBehavior from the spawned projectile, not from the tank
                m_projectile = projectile.GetComponent<ProjectileBehavior>();

                if (m_projectile != null) { m_projectile.p_shootingDeacttivationEvent.AddListener(ReactEventShooting); }
            }
       
        //}
    }

    private void ReactEventShooting()
    {
        m_isShooting = false;
        m_projectile.p_shootingDeacttivationEvent.RemoveListener(ReactEventShooting);
    }

   
}
