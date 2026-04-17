using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    public GameObject t_TankPlayer;

    public int t_MaxPlayers = 4;
    public int t_MinPlayers = 1;
    public int t_currentPlayers;



    public List<GameObject> t_SpawnPoints = new List<GameObject>();


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddPlayers()
    {

    }
}
