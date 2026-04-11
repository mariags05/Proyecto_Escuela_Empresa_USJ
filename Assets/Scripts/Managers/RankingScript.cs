using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RankingScript : MonoBehaviour
{
 
    public ScoreManager m_ScoreManager;
    public Slider[] scoreBoards;

    private int[] oldScore = { 0, 0, 0, 0 };
    private int[] newScore = { 0, 0, 0, 0 };

    

    private void Awake()
    {
        newScore =  m_ScoreManager.GetTotalPlayerScores();
    }
    void Start()
    {
        

        ShowRanking(oldScore, newScore, scoreBoards);
        



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowRanking(int[] oldScoreArray, int[] newScoreArray, Slider[] scoreBoard)
    {
        

        for (int i = 0; i < scoreBoard.Length; i++)
        {
            
            
            scoreBoard[i].value = Mathf.Lerp(oldScoreArray[i], newScoreArray[i], 1);

            oldScore[i] = newScoreArray[i];

        }   
        

    }


}
