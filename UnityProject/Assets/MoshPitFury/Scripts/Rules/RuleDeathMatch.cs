using UnityEngine;
using System.Collections;

public class RuleDeathMatch : Rule {
	
	public override bool IsFinished
	{
		get { return this.Manager.RoundTimer > this.Duration || this.Manager.AlivePlayers.Count <= 1; }
	}

	public override string Description
	{
		get { return "POGO!"; }
	}

	public override Player[] GetWinners()
	{
		if (this.Manager.AlivePlayers.Count == 1)
			return this.Manager.AlivePlayers.ToArray();

		return new Player[0];
	}

	public override void SetAI(Player p, PlayerAI ai)
	{
		ai.SwitchBehaviour<PlayerAIBehaviourIdle>();
	}
}
