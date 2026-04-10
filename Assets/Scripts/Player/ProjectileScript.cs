using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public  class ProjectileScript : MonoBehaviour
{
    // *********************************************************************** //
    //Script done by [Jorge Cristobal]
    //Script for managing the direction and of the proyectile 

    /* State of the Script [ENDED]*/

    //*********************************************************************** //
    
    [SerializeField] private GameObject p_ProyectilePrefab;

    public List<GameObject> p_ProyectilesList = new List<GameObject>();
    public int p_ProyectilePoolSize = 1;
    public float p_ProjectileSpeed = 10f;

    // [SerializeField]  Animator p_Animator;
    [SerializeField] GameObject p_Prefab;

    [SerializeField] int p_MaxCollisions = 3;
    [SerializeField] int p_CurrentCollisions=0;
    

    private static ProjectileScript p_Instance;

    public static ProjectileScript Instance {  get { return p_Instance; } }
    private void Awake()
    {
        if(p_Instance == null)
        {
            p_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        AddProjectileToPool(p_ProyectilePoolSize);
       
    }
     public GameObject RequestProyectile()
    {
        for (int i = 0; i < p_ProyectilesList.Count; i++)
        {
            if (!p_ProyectilesList[i].activeSelf)
            {
                p_ProyectilesList[i].SetActive(true);

                // Resetea el contador de colisiones del proyectil
                ProjectileBehavior behavior = p_ProyectilesList[i].GetComponent<ProjectileBehavior>();
                if (behavior != null) behavior.ResetCollisions();

                return p_ProyectilesList[i];
            }
        }
        Debug.Log("Pool lleno: no hay proyectiles disponibles.");

        GameObject p_Projectile = Instantiate(p_ProyectilePrefab);
        p_Projectile.SetActive(false);
        p_ProyectilesList.Add(p_Projectile);
        p_Projectile.transform.parent = transform;
        return p_Projectile;

    }
    
    private void AddProjectileToPool(int amount)
    {
        for (int i = 0; i < amount; i++) 
        {
            GameObject p_Projectile = Instantiate(p_ProyectilePrefab);
            p_Projectile.SetActive(false);
            p_ProyectilesList.Add(p_Projectile);
            p_Projectile.transform.parent = transform; 

        }
    }
}
