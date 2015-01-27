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
    GameObject[] Players;

	// Use this for initialization
	void Start ()
    {
        Players = new GameObject[4];
        for (int i = 0; i < 4; i++ )
        {
            Players[i] = GameObject.Find("Player_" + (i + 1)) as GameObject;
            Object.DontDestroyOnLoad(Players[i]);
        }

        nbPlayersPanel = GameObject.Find("PanelNBJOUEURS").GetComponent<CanvasRenderer>();
        nbPlayersText = GameObject.Find("TextNBJOUEURS").GetComponent<Text>();
        nbPlayers = 4;
        ShowPlayers();
	}

    public void LessPlayers()
    {
        nbPlayers = Mathf.Max(nbPlayers - 1, 0);
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
            Players[i].SetActive(i < nbPlayers);
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

            Player player = Players[nbPlayersSelected].GetComponentInChildren<Player>();
            player.Id = id;
            player.InitCursor();

            // Switch brains :)
            GameObject.Destroy(player.GetComponent<PlayerAI>());
            player.gameObject.AddComponent<PlayerInput>();

            spotlights[nbPlayersSelected].enabled = true;
                        
            nbPlayersSelected++;
        }
    }

	IEnumerator StartTheGame ()
	{
		yield return new WaitForSeconds(3.0f);

        foreach (GameObject p in Players)
            p.SetActive(true);

		Application.LoadLevel((int)SCENE.Game);
	}
}
