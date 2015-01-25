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
		recordPlayersScore();
		if (Application.loadedLevelName == "FinalScore") {
			if (indexWinner != 1) GameObject.Find("Player_1").GetComponent<PlayerAnimations>().UseAnimation("Loser");
			if (indexWinner != 2) GameObject.Find("Player_2").GetComponent<PlayerAnimations>().UseAnimation("Loser");
			if (indexWinner != 3) GameObject.Find("Player_3").GetComponent<PlayerAnimations>().UseAnimation("Loser");
			if (indexWinner != 4) GameObject.Find("Player_4").GetComponent<PlayerAnimations>().UseAnimation("Loser");
			for (int i=0 ; i < 0 ; i++) {
				transform.GetChild(i).GetComponent<Text>().enabled = true;
				transform.GetChild(i).GetComponent<Text>().text = scores[i].ToString();
			}
		}
	}

	void recordPlayersScore ()
	{
		indexWinner = 1;
		int maxScore = 0;
		for(int i = 0; i < scores.Length; i++)
		{
			if (scores[i] > maxScore)
			{
				maxScore = scores[i];
				indexWinner = (i + 1);
			}
		}
	}


}
