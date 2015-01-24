using UnityEngine;
using System.Collections;

public class RuleKillOnePlayer : Rule {

	private Player PlayerToKill;

	public override void Prepare(GameManager manager)
	{
		this.PlayerToKill = manager.Players[Random.Range(0, manager.Players.Length)];
	}

	public override Transform GetPlayerSpawnPoint(Player p)
	{
		//return base.GetPlayerSpawnPoint(p);

		if(p == PlayerToKill)
		{
			return this.SpawnPoints[0];
		}

		if(p.Id < PlayerToKill.Id)
		{
			return this.SpawnPoints[p.Id];
		}

		return this.SpawnPoints[p.Id - 1];
	}

	public override bool IsFinished
	{
		get 
		{
			return this.Manager.RoundTimer > this.Duration || this.PlayerToKill.IsDead;
		}
	}

	public override string Description
	{
		get { return "KILL PLAYER " + PlayerToKill.Id; }
	}
}
