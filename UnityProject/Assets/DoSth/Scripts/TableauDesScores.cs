using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TableauDesScores : MonoBehaviour {

	int[] playersScore = new int[4];
	int indexWinner = -1;

	void Awake () {
		if (Application.loadedLevelName == "FinalScore") DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		if (Application.loadedLevelName == "FinalScore") {
			if (indexWinner != 1) GameObject.Find("Player_1").SendMessage("UseAnimation", "Player_Loser");
			if (indexWinner != 2) GameObject.Find("Player_2").SendMessage("UseAnimation", "Player_Loser");
			if (indexWinner != 3) GameObject.Find("Player_3").SendMessage("UseAnimation", "Player_Loser");
			if (indexWinner != 4) GameObject.Find("Player_4").SendMessage("UseAnimation", "Player_Loser");
			for (int i=0 ; i < 0 ; i++) {
				transform.GetChild(i).GetComponent<Text>().enabled = true;
				transform.GetChild(i).GetComponent<Text>().text = playersScore[i].ToString();
			}
		}
	}

	void recordPlayersScore () {
		playersScore[0] = GameObject.Find("Player_1").GetComponent<Player>().Score;
		playersScore[1] = GameObject.Find("Player_2").GetComponent<Player>().Score;
		playersScore[2] = GameObject.Find("Player_3").GetComponent<Player>().Score;
		playersScore[3] = GameObject.Find("Player_4").GetComponent<Player>().Score;
		if (playersScore[0] > playersScore[1] && playersScore[0] > playersScore[2] && playersScore[0] > playersScore[3]) indexWinner = 0;
		else if (playersScore[1] > playersScore[0] && playersScore[1] > playersScore[2] && playersScore[1] > playersScore[3]) indexWinner = 1;
		else if (playersScore[2] > playersScore[0] && playersScore[2] > playersScore[1] && playersScore[2] > playersScore[3]) indexWinner = 2;
		else if (playersScore[3] > playersScore[0] && playersScore[3] > playersScore[1] && playersScore[3] > playersScore[2]) indexWinner = 3;
	}


}
