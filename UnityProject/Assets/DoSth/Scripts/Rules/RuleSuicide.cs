using UnityEngine;
using System.Collections;

public class RuleSuicide : Rule 
{
	private Player Winner = null;

	public override string Description
	{
		get { return "KILL YOURSELF!"; }
	}
	
	public override bool IsFinished
	{
		get { return Winner != null || this.Manager.RoundTimer > this.Duration; }
	}

	public override Player[] GetWinners()
	{
		if (Winner == null)
			return new Player[0];

		return new Player[] { Winner };
	}

	public override void Prepare(GameManager manager)
	{
		Winner = null;
	}

	public override void OnPlayerDeath(Player p)
	{
		Winner = p;
	}
}
