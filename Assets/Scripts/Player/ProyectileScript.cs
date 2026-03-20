using UnityEngine;

public abstract class ProyectileScript : MonoBehaviour
{

    //Script done by [Jorge Cristobal]
    //Script for managin the direction and of the proyectile 

    /* State of the Script [IN PROGRESS]*/

    // Remaining things to do:
    // Collider para destruir al jugador o bajarle vidas
    // Poner un contador de destruccion al proyectil que a las X colisiones se destruya
    // La velocidad del proyectil baja con cada colisión
    // Al tocar al jugador o llegar al maximo de colisiones desaparece


    [SerializeField] int p_MaxCollitions;

    void Start()
    {
        
    }

  
    void Update()
    {
        
    }

    void DealDamage()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}
