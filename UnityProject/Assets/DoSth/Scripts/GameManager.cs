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
		UsedRule		= ExistingRules[0]; // à gérer plus tard.
		Players			= GameObject.FindObjectsOfType<Player>();
		AlivePlayers	= new List<Player>(Players);
		
		UsedRule.Start(this);
	}

	void OnPlayerDeath(Player p)
	{
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
