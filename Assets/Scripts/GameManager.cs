using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float MaxTimerDuration;
    private float TimeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        TimeRemaining = MaxTimerDuration;
    }

    // Update is called once per frame
    void Update()
    {
        TimeRemaining -= Time.deltaTime;
    }

    //Restarts the round
    public void RestartRound()
    {

    }
}
