using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleKillOnePlayer : Rule {

	private Player PlayerToKill;
	private List<Player> Killers;

	public override void Prepare(GameManager manager)
	{
		this.PlayerToKill = manager.Players[Random.Range(0, manager.Players.Length)];
        this.PlayerToKill.Halo.color = Color.white;
        this.PlayerToKill.Halo.enabled = true;
        this.Killers = new List<Player>(manager.Players);
		this.Killers.Remove(this.PlayerToKill);
	}

	// public override Transform GetPlayerSpawnPoint(Player p)
	// {
	// 	if(p == PlayerToKill)
	// 	{
	// 		return this.SpawnPoints[0];
	// 	}
	// 
	// 	if(p.Id < PlayerToKill.Id)
	// 	{
	// 		return this.SpawnPoints[p.Id];
	// 	}
	// 
	// 	return this.SpawnPoints[p.Id - 1];
	// }

    public override void GameOver()
    {
        base.GameOver();
        this.PlayerToKill.Halo.enabled = false;
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
		get {
			return "ALL VS <color=" + PlayerToKill.MainColorHex + ">" + PlayerToKill.Name + "</color>"; 
		}
	}

	public override YieldInstruction StartingGame(GameManager manager)
	{
		base.StartingGame(manager);
		HUDTarget.Targets(manager.RuleStartPoints[PlayerToKill.Id - 1].transform);
		return new WaitForSeconds(1);
	}
}
