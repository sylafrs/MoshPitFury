using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class CharacterSelection : MonoBehaviour
{
	public float WaitBeforeLoading = 3;
	bool selectsNbPlayers;
	bool[] selectedCharacters = new bool[4];
	int nbPlayersSelected = 0;
	int nbPlayers;

	Light[] spotlights;
	Text PanelText;
	Text loadingText;
	Text nbPlayersText;
	GameObject PlayersPanels;
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

		spotlights = new Light[4];
		for(int i = 0; i < 4; i++)
			spotlights[i] = GameObject.Find("SpotlightP" + (i + 1)).GetComponent<Light>();

		PanelText = GameObject.Find("PanelText/Text").GetComponent<Text>();
		loadingText = GameObject.Find("TextLoading").GetComponent<Text>();
		nbPlayersText = GameObject.Find("TextNBJOUEURS").GetComponent<Text>();
		PlayersPanels = GameObject.Find("PlayersPanel");

		Transform players = GameObject.Find("Players").transform;
		GameObject.DontDestroyOnLoad(players.gameObject);

		Players = new GameObject[4];
		for (int i = 0; i < 4; i++)
		{
			Players[i] = players.FindChild("Player_" + (i + 1)).gameObject;
		}

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
		PanelText.enabled = false;
		PlayersPanels.SetActive(false);

		for (int i = 0; i < 4; i++)
			ADown[i] = false;

		StartCoroutine(WaitForPlayers());
	}

	IEnumerator WaitForPlayers()
	{
		while (nbPlayersSelected < nbPlayers)
		{
			for (int i = 0; i < 4; i++)
				CheckPlayer(i);

			yield return null;
		}

		StartCoroutine(StartTheGame());
	}

	void CheckPlayer(int i)
	{
		if (!selectedCharacters[i] && ADown[i])
		{
			selectedCharacters[i] = true;

			Player player = Players[nbPlayersSelected].GetComponentInChildren<Player>();
			player.Id = (i+1);
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
			for (int i = 0; i < 4; i++)
			{
				if (!selectedCharacters[i])
				{
					selectedCharacters[i] = true;

					Player player = Players[nbPlayersSelected].GetComponentInChildren<Player>();
					player.Id = (i + 1);
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

		yield return new WaitForSeconds(WaitBeforeLoading);
		
		bool unityPro = Application.HasProLicense();
		Debug.Log("unity pro = " + unityPro);

		PanelText.text = "Loading";
		PanelText.enabled = true;
		loadingText.enabled = true;
		if (unityPro)
		{
			AsyncOperation loadScene = Application.LoadLevelAsync((int)SCENE.Game);
			while(!loadScene.isDone)
			{
				loadingText.text = Mathf.FloorToInt(loadScene.progress * 100).ToString() + '%';
				yield return null;
			}
		}
		else
		{
			Application.LoadLevel((int)SCENE.Game);
		}
	}
}
