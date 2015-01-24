using UnityEngine;
using System.Collections;

public class RuleRace : Rule {
	
	private FinishTrigger Finished;
	
	public override string Name
	{
		get { return "GET TO THE END"; }
	}
	
	public override bool IsFinished
	{
		get { return Finished.Winner; }
	}
	
	public override void StartGame (GameManager manager)
	{
		base.StartGame (manager);
		GameObject.FindObjectOfType<FinishTrigger> ();
	}


}
