using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public Rule[] ExistingRules;

	public Rule UsedRule { get; private set; }

	public List<Player> AlivePlayers { get; private set; }

	private Player[] Players;

	private void Start()
	{
		StartGame();
	}

	public void StartGame()
	{
		Debug.Log("start game");

		UsedRule		= ExistingRules[0]; // à gérer plus tard.
		Players			= GameObject.FindObjectsOfType<Player>();
		AlivePlayers	= new List<Player>(Players);
		
		UsedRule.StartGame(this);
	}

	void OnPlayerDeath(Player p)
	{
		Debug.Log(p.name + " is dead");

		if (AlivePlayers.Contains(p))
			AlivePlayers.Remove(p);
	}

	void Update()
	{
		if(UsedRule != null)
		{
			if(UsedRule.IsFinished)
			{
				GameOver();
			}
		}
	}

	void GameOver()
	{
		StartGame();
	}

}
