using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TableauDesScores : MonoBehaviour {

	public static int[] scores = new int[4];
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
				transform.GetChild(i).GetComponent<Text>().text = scores[i].ToString();
			}
		}
	}

	void recordPlayersScore () {
		scores[0] = GameObject.Find("Player_1").GetComponent<Player>().Score;
		scores[1] = GameObject.Find("Player_2").GetComponent<Player>().Score;
		scores[2] = GameObject.Find("Player_3").GetComponent<Player>().Score;
		scores[3] = GameObject.Find("Player_4").GetComponent<Player>().Score;
		if (scores[0] > scores[1] && scores[0] > scores[2] && scores[0] > scores[3]) indexWinner = 0;
		else if (scores[1] > scores[0] && scores[1] > scores[2] && scores[1] > scores[3]) indexWinner = 1;
		else if (scores[2] > scores[0] && scores[2] > scores[1] && scores[2] > scores[3]) indexWinner = 2;
		else if (scores[3] > scores[0] && scores[3] > scores[1] && scores[3] > scores[2]) indexWinner = 3;
	}


}
