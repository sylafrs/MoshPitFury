using UnityEngine;
using System.Collections;

public class PlayerAI : PlayerBrain
{
	protected PlayerAIBehaviour behaviour;

	public override bool WantToDash
	{
		get { return false; }
	}

	public override Vector3 WantedDirection
	{
		get { return behaviour.WantedDirection; }
	}

	protected override void Awake()
	{
		base.Awake();
		this.behaviour = this.GetComponent<PlayerAIBehaviour>(); //to change


	}

	public override void UpdateState()
	{
		if (this.behaviour)
			this.behaviour.OnUpdate();
	}
}
