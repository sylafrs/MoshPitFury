using UnityEngine;
using System.Collections;

public class RuleDontMove : Rule 
{
	public override string Description
	{
		get { return "Don't move!"; }
	}

	public override void OnPlayerMove(Player p)
	{
		p.Death(false);
	}

	public override Player[] GetWinners()
	{
		return this.Manager.AlivePlayers.ToArray();
	}
}
