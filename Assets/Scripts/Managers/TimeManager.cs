using System.Threading;
using UnityEngine;
using UnityEngine.Events;

// *************************************************************** //
// Script done by Laura
// Tracks the time in the minigames
// In progress
// minigame duration is missing
// *************************************************************** //
public class TimeManager : MonoBehaviour
{
    public float MinigameDuration;//Minigame duration still to be determined
    [SerializeField]private float CurrentTime;
    public UnityEvent TimesUp;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTime = MinigameDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTime > 0)
        {
            timerCountdown();
        } else
        {
            //Activates when timer hits 0
            TimesUp.Invoke();
        }

    }
    void timerCountdown()
    {
        CurrentTime -= Time.deltaTime;
        Debug.Log(CurrentTime);
    }
}
