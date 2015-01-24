using UnityEngine;
using System.Collections;

public class RuleKillOnePlayer : Rule {

	private int PlayerToKill;

	public override void Prepare(GameManager manager)
	{
		this.PlayerToKill = Random.Range(1, manager.AlivePlayers.Count + 1);
	}

	public override bool IsFinished
	{
		get 
		{
			return this.Manager.IsPlayerAlive(PlayerToKill);
		}
	}

	public override string Name
	{
		get { return "KILL PLAYER " + PlayerToKill; }
	}
}
