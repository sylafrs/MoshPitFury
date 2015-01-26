using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class CharacterSelection : MonoBehaviour {

	bool[] selectedCharacters = new bool[4];
	int nbPlayersSelected = 0;
	public Light[] spotlights;
    int nbPlayers;
    CanvasRenderer nbPlayersPanel;
    Text nbPlayersText;
    Player[] Players;

	// Use this for initialization
	void Start ()
    {
        Players = new Player[4];
        for (int i = 0; i < 4; i++ )
        {
            Players[i] = GameObject.Find("Player_" + (i + 1)).GetComponent<Player>();   
        }

        nbPlayersPanel = GameObject.Find("PanelNBJOUEURS").GetComponent<CanvasRenderer>();
        nbPlayersText = GameObject.Find("TextNBJOUEURS").GetComponent<Text>();
        nbPlayers = 2;
        ShowPlayers();
	}

    public void LessPlayers()
    {
        nbPlayers = Mathf.Max(nbPlayers - 1, 2);
        nbPlayersText.text = nbPlayers.ToString();
        ShowPlayers();
    }

    public void MorePlayers()
    {
        nbPlayers = Mathf.Min(nbPlayers + 1, 4);
        nbPlayersText.text = nbPlayers.ToString();
        ShowPlayers();
    }
    
    void ShowPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            Players[i].gameObject.SetActive(i < nbPlayers);
        }
    }

    public void NbPlayersSelected()
    {
        nbPlayersPanel.gameObject.SetActive(false);
        StartCoroutine(WaitForPlayers());
    }

    IEnumerator WaitForPlayers()
    {
        while (nbPlayersSelected < nbPlayers)
        {
            for (int i = 1; i <= nbPlayers; i++)
                CheckPlayer(i);

            yield return null;
        }     

        StartCoroutine(StartTheGame());
    }

    private bool AButton(int id)
    {
        GamePadState padState = GamePad.GetState((PlayerIndex)(id - 1));
        if (padState.IsConnected)
            return padState.Buttons.A == ButtonState.Pressed;

        return Input.GetButton("P" + id + "_A");
    }
    

    void CheckPlayer(int id)
    {
        if (!selectedCharacters[(id - 1)] && AButton(id))
        {
            selectedCharacters[(id - 1)] = true;

            Player p = Players[nbPlayersSelected];
            p.Id = id;

            spotlights[nbPlayersSelected].enabled = true;

            Text playerTextFeedback = GameObject.Find("TextP" + id).GetComponent<Text>();

            playerTextFeedback.enabled = true;
            playerTextFeedback.text = "P" + id;

            Object.DontDestroyOnLoad(p.gameObject);

            nbPlayersSelected++;
        }
    }

	IEnumerator StartTheGame ()
	{
		yield return new WaitForSeconds(3.0f);
		Application.LoadLevel((int)SCENE.Game);
	}
}
