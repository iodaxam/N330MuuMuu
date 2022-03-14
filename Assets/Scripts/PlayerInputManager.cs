using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

public class PlayerInputManager : MonoBehaviour
{
	public GameObject playerAverage;
	
	private GameObject[] m_PlayerList;

	public GameObject AudioManager;
	private AudioManager AudioScript;

	void Start() {
		AudioScript = AudioManager.GetComponent<AudioManager>();
	}

	void OnPlayerJoined()
	{
		UpdatePlayerList();
		AudioScript.Play("Player Join");
	}

	void OnPlayerLeft()
	{
		UpdatePlayerList();
	}

	void UpdatePlayerList()
	{
		m_PlayerList = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log("Current player count: " + m_PlayerList.Length);
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
