using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class CharacterSelection : MonoBehaviour
{
	bool selectsNbPlayers;
	bool[] selectedCharacters = new bool[4];
	int nbPlayersSelected = 0;
	public Light[] spotlights;
	int nbPlayers;
	CanvasRenderer nbPlayersPanel;
	Text nbPlayersText;
	GameObject[] Players;

	private bool[] LeftDown;
	private bool[] RightDown;
	private bool[] Left;
	private bool[] Right;
	private bool[] A;
	private bool[] ADown;

	// Use this for initialization
	void Start()
	{
		LeftDown = new bool[4];
		RightDown = new bool[4];
		Left = new bool[4];
		Right = new bool[4];
		A = new bool[4];
		ADown = new bool[4];

		Transform players = GameObject.Find("Players").transform;
		GameObject.DontDestroyOnLoad(players.gameObject);

		Players = new GameObject[4];
		for (int i = 0; i < 4; i++)
		{
			Players[i] = players.FindChild("Player_" + (i + 1)).gameObject;
		}

		nbPlayersPanel = GameObject.Find("PanelNBJOUEURS").GetComponent<CanvasRenderer>();
		nbPlayersText = GameObject.Find("TextNBJOUEURS").GetComponent<Text>();
		nbPlayers = 4;
		ShowPlayers();

		selectsNbPlayers = true;
		StartCoroutine("SelectNBPlayersControls");
	}

	// void OnGUI()
	// {
	// 	for(int i = 0; i < 4; i++)
	// 	{
	// 		GUILayout.BeginHorizontal();
	// 		if (Left[i])
	// 			GUILayout.Label("Left");
	// 		if (Right[i])
	// 			GUILayout.Label("Right");
	// 		if (A[i])
	// 			GUILayout.Label("A");
	// 		GUILayout.EndHorizontal();
	// 	}
	// }

	void Update()
	{
		float EPSILON = 0.05f;
		float EPSILON_PAD = 0.9f;
		float axis;
		bool aButton;
		for(int i = 0; i < 4; i++)
		{
			GamePadState padState = GamePad.GetState((PlayerIndex)i);

			if (padState.IsConnected)
			{
				EPSILON = EPSILON_PAD;
				axis = padState.ThumbSticks.Left.X;
				aButton = padState.Buttons.A == ButtonState.Pressed;
			}
			else
			{
				axis = Input.GetAxis("P" + (i + 1) + "_Horizontal");
				aButton = Input.GetButton("P" + (i + 1) + "_A");
				//Debug.Log("" + i + ' ' + axis);
			}

			RightDown[i] = false;
			LeftDown[i] = false;
			ADown[i] = false;

			if (axis > EPSILON)
			{
				if (!Right[i])
				{
					RightDown[i] = true;
					Right[i] = true;
				}
			}
			else
			{
				Right[i] = false;
			}

			if (axis < -EPSILON)
			{
				if (!Left[i])
				{
					LeftDown[i] = true;
					Left[i] = true;
				}
			}
			else
			{
				Left[i] = false;
			}

			if (aButton)
			{
				if (!A[i])
				{
					ADown[i] = true;
					A[i] = true;
				}
			}
			else
			{
				A[i] = false;
			}
		}
	}

	IEnumerator SelectNBPlayersControls()
	{
		while (selectsNbPlayers)
		{
			for (int i = 0; i < 4; i++)
			{
				if(ADown[i])
				{
					ADown[i] = false; // Force de nouveau l'appui.
					this.NbPlayersSelected();
					break;
				}

				if(LeftDown[i])
				{
					this.LessPlayers();
				}

				if(RightDown[i])
				{
					this.MorePlayers();
				}
			}

			yield return null;
		}
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
		selectsNbPlayers = false;
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

	void CheckPlayer(int id)
	{
		if (!selectedCharacters[(id - 1)] && ADown[id - 1])
		{
			selectedCharacters[(id - 1)] = true;

			Player player = Players[nbPlayersSelected].GetComponentInChildren<Player>();
			player.Id = id;
			player.InitCursor(false);

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
					player.InitCursor(true);

					player.gameObject.AddComponent<PlayerAI>();

					nbPlayersSelected++;
					break;
				}
			}

		}
	}

	IEnumerator StartTheGame()
	{
		foreach (GameObject p in Players)
			p.SetActive(true);

		CreateCPUS();

		yield return new WaitForSeconds(3.0f);
		Application.LoadLevel((int)SCENE.Game);
	}
}
