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
        Transform players = GameObject.Find("Players").transform;
        GameObject.DontDestroyOnLoad(players.gameObject);

        Players = new GameObject[4];
        for (int i = 0; i < 4; i++ )
        {
            Players[i] = players.FindChild("Player_" + (i + 1)).gameObject;
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
            for (int i = 1; i <= 4; i++)
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
       
            player.gameObject.AddComponent<PlayerInput>();

            spotlights[nbPlayersSelected].enabled = true;
                        
            nbPlayersSelected++;
        }
    }

    void CreateCPUS()
    {
        while (nbPlayersSelected < 4)
        {
            for (int id = 1; id <= 4; id++)
            {
                if (!selectedCharacters[(id - 1)])
                {
                    selectedCharacters[(id - 1)] = true;

                    Player player = Players[nbPlayersSelected].GetComponentInChildren<Player>();
                    player.Id = id;
                    player.InitCPUCursor();

                    player.gameObject.AddComponent<PlayerAI>();

                    nbPlayersSelected++;
                    break;
                }
            }

        }
    }

	IEnumerator StartTheGame ()
    {
        foreach (GameObject p in Players)
            p.SetActive(true);

        CreateCPUS();

		yield return new WaitForSeconds(3.0f);   
		Application.LoadLevel((int)SCENE.Game);
	}
}
