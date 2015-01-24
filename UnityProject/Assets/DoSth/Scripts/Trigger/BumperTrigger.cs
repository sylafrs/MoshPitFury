using UnityEngine;

public class BumperTrigger : MonoBehaviour {

	public float Power;

	void OnTriggerEnter(Collider other)
	{
		other.gameObject.SendMessage("OnBumped", this, SendMessageOptions.DontRequireReceiver);
	}
}
