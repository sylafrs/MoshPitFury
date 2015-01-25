using UnityEngine;
using System.Collections;

public class RuleTheBeer : Rule {

	private PickableBeer Beer;

	public override string Description
	{
		get { return "Rule the beer!"; }
	}

	public override void Prepare(GameManager manager)
	{
		Beer			= (GameObject.Instantiate(this.transform.FindChild("Beer").gameObject) as GameObject).GetComponent<PickableBeer>();
		Beer.Projector	= (GameObject.Instantiate(this.transform.FindChild("Projector").gameObject) as GameObject).GetComponent<ProjectorLookAt>();

		if (Beer.Projector == null)
			throw new UnityException("damn");
	}

	public override void GameOver()
	{
		GameObject.Destroy(Beer.gameObject);
		base.GameOver();
	}

	public override Player[] GetWinners()
	{
		if (Beer.Owner)
		{
			return new Player[] { Beer.Owner };
		}

		return new Player[0];
	}
}
