using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class PlayerInputManager : MonoBehaviour
{
	public static Action MoveToArena;
	
	public GameObject playerAverage;
	public GameObject spawnManager;
	
	private List<PlayerInput> m_PlayerList;

	//Player join event
	public delegate void JoinAction();
	public static event JoinAction PlayerJoin;
	
	private Transform[] Team1Spawns;
	private Transform[] Team2Spawns;
	private Transform[] MenuSpawns;

	private int playerID = 0;

	private void Start()
	{
		SetupSpawnLists();
		GameObject.FindWithTag("GameManager").GetComponent<GameManager>().StartGame += StartGame;
	}

	public void OnPlayerJoined(PlayerInput playerInput)
	{
		m_PlayerList = new List<PlayerInput>();
		Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

		var currentPlayerScript = playerInput.gameObject.GetComponent<PlayerController>();
		currentPlayerScript.playerID = playerID;
		currentPlayerScript.spawnLocation = MenuSpawns[playerID].position;
		Debug.Log("Player added: " + playerInput);
		m_PlayerList.Add(playerInput);
		Debug.Log("Number of children in player list: " + m_PlayerList.Count);
		playerID++;

		//Made this for the audio
		PlayerJoin.Invoke();
	}

	public void OnPlayerLeft()
	{
		m_PlayerList.RemoveAt(playerID);
		playerID--;
	}


	private void SetupSpawnLists()
	{
		Transform[] spawnManagerChildren = new Transform[spawnManager.transform.childCount];
		
		for (int i = 0; i < spawnManagerChildren.Length; i++)
		{
			spawnManagerChildren[i] = spawnManager.transform.GetChild(i);
		}
		
		Team1Spawns = new Transform[spawnManagerChildren[0].transform.childCount];
		Team2Spawns = new Transform[spawnManagerChildren[1].transform.childCount];
		MenuSpawns = new Transform[spawnManagerChildren[2].transform.childCount];

		for (int i = 0; i < Team1Spawns.Length; i++)
		{
			Team1Spawns[i] = spawnManagerChildren[0].transform.GetChild(i);
		}
		for (int i = 0; i < Team2Spawns.Length; i++)
		{
			Team2Spawns[i] = spawnManagerChildren[1].transform.GetChild(i);
		}
		for (int i = 0; i < MenuSpawns.Length; i++)
		{
			MenuSpawns[i] = spawnManagerChildren[2].transform.GetChild(i);
		}
	}

	private void Update()
	{
		var playerPositions = new List<Vector3>();

		// Make the camera follow the average between the players.
		if (m_PlayerList == null) return;
		playerPositions.AddRange(m_PlayerList.Select(player => player.gameObject.transform.position));
		playerAverage.transform.position = playerPositions.Aggregate(new Vector3(0,0,0), (s,v) => s + v) / (float)playerPositions.Count;
	}

	private void StartGame()
	{
		// When it's time to start the game, make all the players move to the arena
		foreach (PlayerInput player in m_PlayerList)
		{
			Random spawn = new Random();
			player.gameObject.GetComponent<PlayerController>().spawnLocation = (player.playerIndex % 2) switch
			{
				0 => Team1Spawns[spawn.Next(1, 2)].position,
				1 => Team2Spawns[spawn.Next(1, 2)].position,
				_ => player.gameObject.GetComponent<PlayerController>().spawnLocation
			};
			Debug.Log("Setting Spawns");
		}
		MoveToArena?.Invoke();
	}
}
