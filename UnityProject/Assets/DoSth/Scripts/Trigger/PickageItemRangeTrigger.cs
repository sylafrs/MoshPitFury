using UnityEngine;
using System.Collections;

public class PickageItemRange : MonoBehaviour 
{
	public string message;

	public virtual bool CanTake
	{
		get
		{
			return true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.name + " in range");
		if(CanTake)
			other.gameObject.SendMessage(message, this, SendMessageOptions.DontRequireReceiver);
	}
}
