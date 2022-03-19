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
	public GameObject playerAverage;
	public GameObject spawnManager;
	
	private GameObject[] m_PlayerList;

	public GameObject AudioManager;
	private AudioManager AudioScript;
	
	private Transform[] Team1Spawns;
	private Transform[] Team2Spawns;
	private Transform[] MenuSpawns;

	private int nextPlayerID = 0;

	private void Start()
	{
		SetupSpawnLists();
		AudioScript = AudioManager.GetComponent<AudioManager>();
	}

	public void OnPlayerJoined(PlayerInput playerInput)
	{
		UpdatePlayerList();
		Debug.Log("PlayerInput ID: " + playerInput.playerIndex);
		Random spawnLocation = new Random();

		var currentPlayerScript = playerInput.gameObject.GetComponent<PlayerController>();
		currentPlayerScript.playerID = nextPlayerID;
		currentPlayerScript.spawnLocation = MenuSpawns[nextPlayerID].position;
		nextPlayerID++;

		// AudioScript.Play("Player Join"); // may be better to call an event that the GameManager subscribes to and leave audio out of this script altogether
	}

	public void OnPlayerLeft()
	{
		UpdatePlayerList();
		nextPlayerID--;
	}

	void UpdatePlayerList()
	{
		m_PlayerList = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log("Current player count: " + m_PlayerList.Length);
	}

	void SetupSpawnLists()
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
		
		List<Vector3> playerPositions = new List<Vector3>();

		if (m_PlayerList != null)
		{
			foreach (GameObject player in m_PlayerList)
			{
				playerPositions.Add(player.transform.position);
			}
			playerAverage.transform.position = playerPositions.Aggregate(new Vector3(0,0,0), (s,v) => s + v) / (float)playerPositions.Count;
		}

	}
}
