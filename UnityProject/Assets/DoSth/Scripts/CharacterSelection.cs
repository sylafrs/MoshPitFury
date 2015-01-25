using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class CharacterSelection : MonoBehaviour {

	bool[] selectedCharacters = new bool[4];
	int nbDots = 0, nbPlayersSelected = 0;
	Text[] displayFeedback;
	GameObject[] players;
	string[] playerName = new string[4];
	public Light[] spotlights;

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");
		displayFeedback = GetComponentsInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		int nbGamepads = Input.GetJoystickNames().GetLength(0);
		//selectedCharacters = new bool[nbGamepads];
		//Debug.Log(GamePad.GetState(PlayerIndex.One).Buttons.A);
		if (nbPlayersSelected < 4) {
			if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && !selectedCharacters[0]) {
				selectedCharacters[0] = true;
				players[nbPlayersSelected].GetComponent<Player>().Id = 1;
				playerName[nbPlayersSelected] = "1";
				
				spotlights[nbPlayersSelected].enabled = true;
				Text playerTextFeedback = transform.GetChild(nbPlayersSelected).GetComponent<Text>();
				playerTextFeedback.enabled = true;
				playerTextFeedback.text = "P"+playerName[nbPlayersSelected];

				nbPlayersSelected++;
			}
			if (GamePad.GetState(PlayerIndex.Two).Buttons.Start == ButtonState.Pressed && !selectedCharacters[1]) {
				selectedCharacters[1] = true;
				players[nbPlayersSelected].GetComponent<Player>().Id = 2;
				playerName[nbPlayersSelected] = "2";
				
				spotlights[nbPlayersSelected].enabled = true;
				Text playerTextFeedback = transform.GetChild(nbPlayersSelected).GetComponent<Text>();
				playerTextFeedback.enabled = true;
				playerTextFeedback.text = "P"+playerName[nbPlayersSelected];

				nbPlayersSelected++;
			}
			if (GamePad.GetState(PlayerIndex.Three).Buttons.Start == ButtonState.Pressed && !selectedCharacters[2]) {
				selectedCharacters[2] = true;
				players[nbPlayersSelected].GetComponent<Player>().Id = 3;
				playerName[nbPlayersSelected] = "3";
				
				spotlights[nbPlayersSelected].enabled = true;
				Text playerTextFeedback = transform.GetChild(nbPlayersSelected).GetComponent<Text>();
				playerTextFeedback.enabled = true;
				playerTextFeedback.text = "P"+playerName[nbPlayersSelected];

				nbPlayersSelected++;
			}
			if (GamePad.GetState(PlayerIndex.Four).Buttons.Start == ButtonState.Pressed && !selectedCharacters[3]) {
				selectedCharacters[3] = true;
				players[nbPlayersSelected].GetComponent<Player>().Id = 4;
				playerName[nbPlayersSelected] = "4";
				
				spotlights[nbPlayersSelected].enabled = true;
				Text playerTextFeedback = transform.GetChild(nbPlayersSelected).GetComponent<Text>();
				playerTextFeedback.enabled = true;
				playerTextFeedback.text = "P"+playerName[nbPlayersSelected];

				nbPlayersSelected++;
			}
		}

		/*

		for (int i=0 ; i < nbPlayersSelected ; i++) {
		}
		for (int i=0 ; i < nbGamepads ; i++) {
			if (!selectedCharacters[i]) {
				displayFeedback[i].text = "Waiting";
				for (int j=0 ; j < Mathf.FloorToInt((float) nbDots/20.0f) ; j++) {
					displayFeedback[i].text += ".";
				}
				nbDots++;
				if (nbDots >= 80) nbDots = 0;
			}
			else displayFeedback[i].text = "Ok";
		}
		*/
	}
}
