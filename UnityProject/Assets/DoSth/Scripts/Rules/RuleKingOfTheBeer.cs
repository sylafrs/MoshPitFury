using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleKingOfTheBeer : Rule {

	public override string Description
	{
		get { return "BUVEZ LE PLUS !"; }
	}

	private Dictionary<Player, float> Times;
	private GameObject Area;

	public override void Prepare(GameManager manager)
	{
		Times = new Dictionary<Player, float>();
		foreach(Player p in manager.Players)
		{
			Times.Add(p, 0);
		}

		Area = GameObject.Instantiate(this.transform.FindChild("BeerArea").gameObject) as GameObject;
	}

	void OnPlayerStayInBeerArea(Player p)
	{
		Times[p] += Time.deltaTime;
	}

	public override void GameOver()
	{
		base.GameOver();
		GameObject.Destroy(Area);
	}
}
