using UnityEngine;
using System.Collections;

public class BeerMissile : MonoBehaviour {

	private MonoBehaviour callback;
	
	void OnSetCallback(MonoBehaviour g)
	{
		callback = g;
	}

	void OnCollisionEnter(Collision c)
	{
		c.gameObject.SendMessage("OnBeerCollision", c, SendMessageOptions.DontRequireReceiver);
		this.Destroy();
	}

	void Destroy()
	{
		if(callback != null)
			callback.SendMessage("OnBeerDestroyed", this.gameObject);

		GameObject.Destroy(this.gameObject);
	}

}
