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

	void Update()
	{
		if (this.transform.position.y < 0)
			Destroy();
	}

	void Destroy()
	{
		if(callback != null)
			callback.SendMessage("OnBeerDestroyed", this.gameObject);

		this.gameObject.SendMessage("OnBeerDestroyed", SendMessageOptions.DontRequireReceiver);

		GameObject.Destroy(this.gameObject);
	}

}
