using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider c)
	{
		c.gameObject.SendMessage("OnDeathTrigger", this, SendMessageOptions.DontRequireReceiver);
	}
}
