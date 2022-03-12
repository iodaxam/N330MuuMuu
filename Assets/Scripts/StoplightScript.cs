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

    public float LightIntensity = 1f;

    private void Start() 
    {
        RedLight = RedLightObject.GetComponent<Light>();
        YellowLight = YellowLightObject.GetComponent<Light>();
        GreenLight = GreenLightObject.GetComponent<Light>();
    }

    public void ChangeIntensity(EnumStopLight NewLight)
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
