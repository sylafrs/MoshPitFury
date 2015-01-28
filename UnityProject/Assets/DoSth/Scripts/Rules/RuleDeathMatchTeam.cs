using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleDeathMatchTeam : Rule
{

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

	public override Player[] GetWinners()
	{
		bool teamADead = this.IsTeamDead(teamA);
		bool teamBDead = this.IsTeamDead(teamB);

		// Personne en vie
		if (teamADead && teamBDead)
			return new Player[0];

		// que B
		if (teamADead)
			return teamB.ToArray();

		// que A
		if (teamBDead)
			return teamA.ToArray();

		// Tout le monde vivant
		return new Player[0];
	}

	private string ListTeam(List<Player> team)
	{
		string str = "Players ";

		bool first = true;
		foreach (Player p in team)
		{
			if (!first)
				str += " / ";
			first = false;
			str += "<color=" + p.MainColorHex + ">" + p.Name + "</color>";
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

		for (int i = 0; i < teamSize; i++)
		{
			chosen = remaining[Random.Range(0, remaining.Count)];
			teamA.Add(chosen);
			chosen.Halo.color = Color.red;
			chosen.Halo.enabled = true;
			remaining.Remove(chosen);
		}

		for (int i = 0; i < teamSize; i++)
		{
			chosen = remaining[Random.Range(0, remaining.Count)];
			teamB.Add(chosen);
			chosen.Halo.color = Color.green;
			chosen.Halo.enabled = true;
			remaining.Remove(chosen);
		}

		base.Prepare(manager);
	}

	public override void GameOver()
	{
		foreach (Player p in this.Manager.Players)
		{
			p.Halo.color = Color.white;
			p.Halo.enabled = false;
		}
		base.GameOver();
	}

	private bool IsTeamDead(List<Player> team)
	{
		foreach (Player p in team)
		{
			if (!p.IsDead)
				return false;
		}

		return true;
	}
}
