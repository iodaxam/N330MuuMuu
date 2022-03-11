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

    private void Start() 
    {
        RedLight = RedLightObject.GetComponent<Light>();
        YellowLight = YellowLightObject.GetComponent<Light>();
        GreenLight = GreenLightObject.GetComponent<Light>();
    }

    private IEnumerator StopLightTimer(EnumStopLight CurrentLightState)
    {
        Debug.Log("Running");
        // switch (CurrentLightState)
        // {
        //     case EnumStopLight.RedLightState:
        //         break;
        //     case EnumStopLight.YellowLightState:
        //         break;
        //     case EnumStopLight.GreenLightState:
        //         break;
        // }
        yield return null;
    }
}
