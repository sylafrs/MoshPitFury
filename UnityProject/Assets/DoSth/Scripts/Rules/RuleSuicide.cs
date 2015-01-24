using UnityEngine;
using System.Collections;

public class RuleSuicide : Rule {

	public override string Name
	{
		get { return "GET TO THE END"; }
	}
	
	public override bool IsFinished
	{
		get { return Input.GetKeyDown(KeyCode.Space); }
	}
}
