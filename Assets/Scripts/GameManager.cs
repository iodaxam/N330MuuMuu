using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumStopLight {RedLightState, YellowLightState, GreenLightState};

public class GameManager : MonoBehaviour
{
    public GameObject AudioManager;
    private AudioManager AudioScript;

    private GameObject[] StopLights;

    public EnumStopLight StopLightState;

    private float Timer;
    public float GreenLightMaxTime;
    public float GreenLightMinTime;
    public float YellowLightTime = 2f;
    public float RedLightTime = 3f;

    public float DamageDrain = .01f;

    public float LightIntensity = 1f;

    void Start()
    {
        AudioScript = AudioManager.GetComponent<AudioManager>();

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
                    AudioScript.Stop("Crosswalk Beep");
                    AudioScript.Play("Background Jazz");
                    break;
                case EnumStopLight.YellowLightState:
                    StopLightState = EnumStopLight.RedLightState;
                    AudioScript.Play("Crosswalk Beep");
                    AudioScript.Stop("Background Jazz");
                    Timer = RedLightTime;
                    break;
                case EnumStopLight.GreenLightState:
                    StopLightState = EnumStopLight.YellowLightState;
                    Timer = YellowLightTime;
                    break;
            }

            AudioScript.Play("Light Switch");

            foreach(GameObject trafficLight in StopLights)
            {
                trafficLight.SendMessage("ChangeIntensity", StopLightState);
            }
        }
    }
}
