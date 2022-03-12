using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumStopLight {RedLightState, YellowLightState, GreenLightState};

public class GameManager : MonoBehaviour
{
    private GameObject[] StopLights;

    private EnumStopLight StopLightState;

    private float Timer;
    public float GreenLightMaxTime;
    public float GreenLightMinTime;
    public float YellowLightTime = 2f;
    public float RedLightTime = 3f;

    public float LightIntensity = 1f;

    void Start()
    {
        StopLights = GameObject.FindGameObjectsWithTag("StopLight");

        StopLightState = EnumStopLight.GreenLightState;

        Timer = Random.Range(GreenLightMinTime, GreenLightMaxTime);

        foreach(GameObject trafficLight in StopLights)
        {
            trafficLight.SendMessage("ChangeIntensity", StopLightState);
        }
    }

    void Update()
    {
        StopLightTimer();
    }

    private void StopLightTimer()
    {
        Timer -= Time.deltaTime;

        if(Timer <= 0f)
        {
            switch (StopLightState)
            {
                case EnumStopLight.RedLightState:
                    StopLightState = EnumStopLight.GreenLightState;
                    Timer = Random.Range(GreenLightMinTime, GreenLightMaxTime);
                    break;
                case EnumStopLight.YellowLightState:
                    StopLightState = EnumStopLight.RedLightState;
                    Timer = RedLightTime;
                    break;
                case EnumStopLight.GreenLightState:
                    StopLightState = EnumStopLight.YellowLightState;
                    Timer = YellowLightTime;
                    break;
            }

            foreach(GameObject trafficLight in StopLights)
            {
                trafficLight.SendMessage("ChangeIntensity", StopLightState);
            }
        }
    }
}
