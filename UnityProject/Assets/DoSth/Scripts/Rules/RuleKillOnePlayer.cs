using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleKillOnePlayer : Rule {

	private Player PlayerToKill;
	private List<Player> Killers;

	public override void Prepare(GameManager manager)
	{
		this.PlayerToKill = manager.Players[Random.Range(0, manager.Players.Length)];
		this.Killers = new List<Player>(manager.Players);
		this.Killers.Remove(this.PlayerToKill);
	}

	public override Transform GetPlayerSpawnPoint(Player p)
	{
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

	public override Player[] GetWinners()
	{
		if(this.PlayerToKill.IsDead)
		{
			return this.Killers.ToArray();
		}

		return new Player[] { this.PlayerToKill };
	}

	public override string Description
	{
		get { return "ALL VS PLAYER <color=" + PlayerToKill.MainColorHex + ">" + PlayerToKill.Id + "</color>"; }
	}
}
