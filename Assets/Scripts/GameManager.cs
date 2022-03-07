using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float LightDuration;
    private float TimeRemaining;
    
    public float MaxLightDuration;
    public float MinLightDuration;

    private enum EnumStopLight {RedLight, YellowLight, GreenLight};
    private EnumStopLight StopLight;
    
    private void SetLightTimer(EnumStopLight CurrentLight) 
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        StopLight = EnumStopLight.GreenLight;
        SetLightTimer(StopLight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Restarts the round
    public void RestartRound()
    {

    }
}
