using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider c)
	{
		Debug.Log(c.name + " has cross a wrong trigger, Mwahahahhaha !!!");
		c.gameObject.SendMessage("OnDeathTrigger", this, SendMessageOptions.DontRequireReceiver);
	}
}
