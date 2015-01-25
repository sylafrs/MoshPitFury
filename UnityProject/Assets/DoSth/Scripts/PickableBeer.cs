using UnityEngine;
using System.Collections;

public class PickableBeer : PickageItemRange 
{
	[HideInInspector]
	public Player Owner;

	[HideInInspector]
	public ProjectorLookAt Projector;

	public override bool CanTake
	{
		get
		{
			return Owner == null;
		}
	}

	void OnDestroy()
	{
		GameObject.Destroy(Projector.gameObject);

		if(Owner)
			Owner.gameObject.SendMessage("OnBeerDestroyed");
	}
}
