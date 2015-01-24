using UnityEngine;
using System.Collections;

public class RuleSuicide : Rule 
{
	private Player Winner = null;

	public override string Description
	{
		get { return "DIE."; }
	}
	
	public override bool IsFinished
	{
		get { return Winner != null || this.Manager.RoundTimer > this.Duration; }
	}

	public override void Prepare(GameManager manager)
	{
		Winner = null;
	}
	void OnPlayerDeath(Player p)
	{
		Winner = p;
	}
}
