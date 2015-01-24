using UnityEngine;
using System.Collections;

public class RuleDeathMatch : Rule {
	
	public override bool IsFinished
	{
		get { return this.Manager.AlivePlayers.Count <= 1; }
	}

	public override string Name
	{
		get { return "DEATHMATCH"; }
	}
}
