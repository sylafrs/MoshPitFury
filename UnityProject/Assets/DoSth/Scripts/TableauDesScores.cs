using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TableauDesScores : MonoBehaviour {

	public static int[] scores = new int[4];
    int maxScore = 0;
	
	void Start ()
    {
        int maxScore = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > maxScore)
            {
                maxScore = scores[i];
            }
        }

	    for (int i = 0; i < 4; i++)
        {
            if (scores[i] < maxScore)
                Looser(i);
        }
		
	}

    void Looser(int id)
    {
        GameObject go = GameObject.Find("Player_" + (id + 1));
        if (go)
        {
            PlayerAnimations pa = go.GetComponent<PlayerAnimations>();
            if(pa)
                pa.UseAnimation("Loser");
        }
    }

    public void MainMenu()
    {
        Application.LoadLevel(0);
    }
}
