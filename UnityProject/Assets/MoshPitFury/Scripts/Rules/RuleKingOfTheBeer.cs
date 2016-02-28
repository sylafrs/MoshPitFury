using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleKingOfTheBeer : Rule
{

    //public Sprite beerCrown;

	public override string Description
	{
		get { return "KEEP THE BEERS"; }
	}

	private Dictionary<Player, float> Times;
	private GameObject Area;

	public override void Prepare(GameManager manager)
	{
		Times = new Dictionary<Player, float>();
		foreach (Player p in manager.Players)
		{
			Times.Add(p, 0);
		}

		Area = GameObject.Instantiate(this.transform.FindChild("BeerArea").gameObject) as GameObject;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		float winningTime = GetMaxScore();
		foreach (Player p in this.Manager.Players)
		{
			p.Halo.enabled = (this.Times[p] >= winningTime);
		}
	}

	public override void OnPlayerStayInBeerArea(Player p)
	{
		Times[p] += Time.deltaTime;
	}

	public override void GameOver()
	{
		foreach (Player p in this.Manager.Players)
		{
			p.Halo.enabled = false;
		}
		GameObject.Destroy(Area);
		base.GameOver();
	}

	public float GetMaxScore()
	{
		float max = 0;

		foreach (float t in this.Times.Values)
		{
			if (t > max)
				max = t;
		}

		return max;
	}

	public override Player[] GetWinners()
	{
		List<Player> winners = new List<Player>();
		float winningTime = GetMaxScore();

		foreach (Player p in this.Manager.Players)
		{
			if (this.Times[p] >= winningTime)
			{
				winners.Add(p);
			}
		}

		return winners.ToArray();
	}

	public override void SetAI(Player p, PlayerAI ai)
	{
		ai.SwitchBehaviour<PlayerAIBehaviourIdle>();
	}
}
