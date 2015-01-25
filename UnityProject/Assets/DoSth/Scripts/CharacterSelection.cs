using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class CharacterSelection : MonoBehaviour {

	bool[] selectedCharacters = new bool[4];
	int nbPlayersSelected = 0;
	public Light[] spotlights;
	int nbManettes;

	// Use this for initialization
	void Start () 
	{
		nbManettes = Input.GetJoystickNames().GetLength(0);
		if (nbManettes < 4) 
		{
			if (!GamePad.GetState(PlayerIndex.One).IsConnected) Debug.LogError("NO.");
			if (!GamePad.GetState(PlayerIndex.Two).IsConnected) Debug.LogError("NO.");
			if (!GamePad.GetState(PlayerIndex.Three).IsConnected) GameObject.Find("Player_3").SetActive(false);
			if (!GamePad.GetState(PlayerIndex.Four).IsConnected) GameObject.Find("Player_4").SetActive(false);
		}
	}

	void CheckPlayer(int id)
	{
		if (!selectedCharacters[(id - 1)] && GamePad.GetState((PlayerIndex)(id - 1)).Buttons.Start == ButtonState.Pressed)
		{
			selectedCharacters[(id - 1)] = true;

			GameObject p = GameObject.Find("Player_" + (nbPlayersSelected + 1));
			p.GetComponent<Player>().Id = id;

			spotlights[nbPlayersSelected].enabled = true;

			Text playerTextFeedback = transform.GetChild(nbPlayersSelected).GetComponent<Text>();
			playerTextFeedback.enabled = true;
			playerTextFeedback.text = "P" + id;

			Object.DontDestroyOnLoad(p);

			nbPlayersSelected++;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (nbPlayersSelected < nbManettes)
		{
			for (int i = 1; i <= nbManettes; i++)
				CheckPlayer(i);
		}
		else
		{
			StartCoroutine(startTheGame());
		}

		/*
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

	IEnumerator startTheGame ()
	{
		yield return new WaitForSeconds(3.0f);
		Application.LoadLevel((int)SCENE.Game);
	}
}
