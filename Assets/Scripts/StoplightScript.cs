using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoplightScript : MonoBehaviour
{
    public GameObject RedLightObject;
    public GameObject YellowLightObject;
    public GameObject GreenLightObject;

    private Light RedLight;
    private Light YellowLight;
    private Light GreenLight;

    private enum EnumStopLight {RedLightState, YellowLightState, GreenLightState};
    private EnumStopLight StopLightState;

    private float Timer;
    public float GreenLightMaxTime;
    public float GreenLightMinTime;
    public float YellowLightTime = 2f;
    public float RedLightTime = 3f;

    public float LightIntensity = 1f;

    private void Start() 
    {
        RedLight = RedLightObject.GetComponent<Light>();
        YellowLight = YellowLightObject.GetComponent<Light>();
        GreenLight = GreenLightObject.GetComponent<Light>();

        StopLightState = EnumStopLight.GreenLightState;

        Timer = Random.Range(GreenLightMinTime, GreenLightMaxTime);

        ChangeIntensity(StopLightState);
    }

    private void Update()
    {
        StopLightTimer();
        Debug.Log(StopLightState);
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

            ChangeIntensity(StopLightState);
        }
    }

    private void ChangeIntensity(EnumStopLight NewLight)
    {
        switch(NewLight)
        {
            case EnumStopLight.RedLightState:
                RedLight.intensity = LightIntensity;
                YellowLight.intensity = 0f;
                break;
            case EnumStopLight.YellowLightState:
                YellowLight.intensity = LightIntensity;
                GreenLight.intensity = 0f;
                break;
            case EnumStopLight.GreenLightState:
                GreenLight.intensity = LightIntensity;
                RedLight.intensity = 0f;
                break;
        }
    }
}
