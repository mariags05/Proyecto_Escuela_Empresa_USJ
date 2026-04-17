using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class RankingScript : MonoBehaviour
{
 
    public ScoreManager m_ScoreManager;
    public Slider[] scoreBoards;

    public int[] oldScore = { 0, 0, 0, 0 };
    public int[] newScore = { 0, 0, 0, 0 };
    private float m_Speed = 0.1f;
   
    float timeScale = 0f;

    

    private void Awake()
    {
        // newScore =  m_ScoreManager.GetTotalPlayerScores();
    }
    void Update()
    {
        
        StartCoroutine (RankingView(scoreBoards, oldScore, newScore));
    }

    
    private IEnumerator RankingView(Slider[] score, int[] old, int[] actual) 
    {
        
       
        for(int i = 0;i < score.Length; i++)
        {
            while (timeScale < 1)
            {
                timeScale += Time.deltaTime * m_Speed;
                score[i].value = Mathf.Lerp(old[i], actual[i], timeScale);
            }
            timeScale = 0f;
            yield return new WaitForSeconds(2);
        }
      
    }
}
