using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public enum EnumStopLight {RedLightState, YellowLightState, GreenLightState};

public class GameManager : MonoBehaviour
{
    public delegate void RestartAction();
    public event RestartAction ResetPlayers;

    public Action StartGame;
    
    [Header("Cameras")] 
    public Camera MainCam;
    public Camera MenuCam;
    
    public GameObject AudioManager;
    private AudioManager AudioScript;

    [Header("Menus")] public Canvas MainMenu;

    private GameObject[] StopLights;

    public EnumStopLight StopLightState;
    
    private float Timer;
    public float GreenLightMaxTime;
    public float GreenLightMinTime;
    public float YellowLightTime = 2f;
    public float RedLightTime = 3f;

    public float DamageDrain = .01f;

    public float LightIntensity = 1f;

    //Game timer
    // public float GameTimer = 30f;
    // private float CurrentGameTimer;
    // public GameObject TimerTextObject;
    // private Text TimerTextComponent;

    private int readyPlayers = 0;

    private bool GameStarted = false;

    public GameObject MenuCanvas;

    private float RestartTimer;

    public GameObject InputManagerPrefab;
    private PlayerInputManager InputScript;

    void OnEnable()
    {
        PlayerInputManager.PlayerJoin += PlayerJoined;
    }

    void OnDisable()
    {
        PlayerInputManager.PlayerJoin -= PlayerJoined;
    }

    void Start()
    {
        InputScript = InputManagerPrefab.GetComponent<PlayerInputManager>();
        MenuCam.enabled = true;
        MainCam.enabled = false;
        
        AudioScript = AudioManager.GetComponent<AudioManager>();

        StopLights = GameObject.FindGameObjectsWithTag("StopLight");

        StopLightState = EnumStopLight.GreenLightState;

        Timer = Random.Range(GreenLightMinTime, GreenLightMaxTime);

        // TimerTextComponent = TimerTextObject.GetComponent<Text>();

        foreach(GameObject trafficLight in StopLights)
        {
            trafficLight.SendMessage("ChangeIntensity", StopLightState);
        }

        // CurrentGameTimer = GameTimer;
    }

    void Update()
    {
        if(GameStarted)
        {
            StopLightTimer();
        }

        if(RestartTimer >= 0f)
        {
            RestartTimer -= Time.deltaTime;

            if(RestartTimer <= 0f)
            {
                RestartRound();
            }
        }

        //Game Timer

        // if(CurrentGameTimer > 0f)
        // {
        //     CurrentGameTimer -= Time.deltaTime;

        //     TimerTextComponent.text = Mathf.RoundToInt(CurrentGameTimer).ToString();
        //     if(Mathf.RoundToInt(CurrentGameTimer) == 10)
        //     {
        //         TimerTextComponent.color = Color.red;
        //     }
        // }
        // else if(CurrentGameTimer <= 0f)
        // {
        //     Debug.Log("Game Over");
        // }
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
    
    public void Ready()
    {
        readyPlayers++;
        /*if (readyPlayers == GameObject.FindGameObjectsWithTag("Player").Length && readyPlayers > 1)
        {*/
            StartGame?.Invoke();
            MenuCam.enabled = !MenuCam.enabled;
            MainCam.enabled = !MainCam.enabled;
            GameStarted = true;

            Destroy(MenuCanvas);
            //make the ui invisible here

        /*} 
        else if (readyPlayers <= 1) 
        {
            Debug.Log("Need at least 2 players");
        }*/
    }

    void PlayerJoined()
    {
        AudioScript.Play("Player Join");
        MainMenu.enabled = false;
    }

    public void PlayerHitFunction(bool LightAttack)
    {
        String[] vocalHits = { "PlayerHit", "PlayerHit1", "PlayerHit2", "PlayerHit3" };
        String[] lHits = { "LightHit1", "LightHit2"};
        int vocalHit = Random.Range(0, vocalHits.Length);
        int lHit = Random.Range(0, lHits.Length);

        if(LightAttack)
        {
            AudioScript.Play(lHits[lHit]);
        } else
        {
            AudioScript.Play(vocalHits[vocalHit]);
        }


        // int i = 0;
        // foreach (String sound in sounds) {
        //     i++;
        //     switch(i)
        //     {
        //         case 0:
        //             AudioScript.Play(sounds[0]);
        //             break;
        //         case 1:
        //             AudioScript.Play(sounds[1]);
        //             break;
        //         case 2:
        //             AudioScript.Play(sounds[2]);
        //             break;
        //         case 3:
        //             AudioScript.Play(sounds[3]);
        //             break;
        //     }
        // }
    }

    public void PlayerDied()
    {
        int DeathSound = Random.Range(0,1);
        switch(DeathSound)
        {
            case 0:
                AudioScript.Play("DeathSound1");
                break;
            case 1:
                AudioScript.Play("DeathSound2");
                break;
        }

        RestartTimer = 2f;
    }

    public void RestartRound()
    {
        InputScript.StartGame();
        ResetPlayers.Invoke();
    }
}
