using UnityEngine;
using System.Collections;

public class GroundTrigger : MonoBehaviour {

	void OnTriggerExit(Collider c)
	{
		c.gameObject.SendMessage("OnGroundLost", SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerEnter(Collider c)
	{
		c.gameObject.SendMessage("OnGroundReached", SendMessageOptions.DontRequireReceiver);
	}
}
