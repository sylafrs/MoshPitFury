using UnityEngine;
using System.Collections;

public class RuleDontMove : Rule 
{
	public override string Description
	{
		get { return "Don't move."; }
	}

	void OnPlayerMove(Player p)
	{
		p.Death();
	}
	
}
