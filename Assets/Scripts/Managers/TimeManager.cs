// *************************************************************** //
// Script done by Laura
// Tracks the time in the minigames
// In progress
// minigame duration is missing
// *************************************************************** //
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public float m_MinigameDuration;//Minigame duration still to be determined
    [SerializeField]private float m_CurrentTime;
    public UnityEvent m_TimesUp;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_CurrentTime = m_MinigameDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrentTime > 0)
        {
            timerCountdown();
        } else
        {
            //Activates when timer hits 0
            m_TimesUp.Invoke();
        }

    }
    void timerCountdown()
    {
        m_CurrentTime -= Time.deltaTime;
        Debug.Log(m_CurrentTime);
    }
}
