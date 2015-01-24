using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleDeathMatchTeam : Rule {

	private List<Player> teamA;
	private List<Player> teamB;

	public override string Description
	{
		get { return ListTeam(teamA) + "\nVS\n" + ListTeam(teamB); }
	}

	public override bool IsFinished
	{
		get
		{
			return this.Manager.RoundTimer > this.Duration || this.IsTeamDead(teamA) || this.IsTeamDead(teamB);
		}
	}

	private string ListTeam(List<Player> team)
	{
		string str = "";

		bool first = true;
		foreach(Player p in team)
		{
			if (!first)
				str += " / ";
			first = false;
			str += "Player " + p.Id;
		}

		return str;
	}

	public override void Prepare(GameManager manager)
	{
		Player chosen;
		List<Player> remaining = new List<Player>(manager.Players);
		int teamSize = remaining.Count / 2;

		teamA = new List<Player>();
		teamB = new List<Player>();

		for (int i = 0; i < teamSize; i++ )
		{
			chosen = remaining[Random.Range(0, remaining.Count)];
			teamA.Add(chosen);
			remaining.Remove(chosen);
		}

		for (int i = 0; i < teamSize; i++)
		{
			chosen = remaining[Random.Range(0, remaining.Count)];
			teamB.Add(chosen);
			remaining.Remove(chosen);
		}

		base.Prepare(manager);
	}

	private bool IsTeamDead(List<Player> team)
	{
		foreach(Player p in team)
		{
			if (!p.IsDead)
				return false;
		}

		return true;
	}
}
