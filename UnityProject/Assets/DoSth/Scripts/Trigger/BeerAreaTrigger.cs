using UnityEngine;
using System.Collections;

public class BeerAreaTrigger : MonoBehaviour {

	public void OnTriggerStay(Collider c)
	{
		c.SendMessage("OnBeerAreaStay", this, SendMessageOptions.DontRequireReceiver);
	}

}
