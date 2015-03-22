using UnityEngine;
using System.Collections;

public class DetachThenFollow : MonoBehaviour {

	private Transform toFollow;
	private Vector3 offset;

	void Start () {
		toFollow = transform.parent;
		offset = transform.position - toFollow.position;
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (toFollow == null)
			GameObject.Destroy(this.gameObject);
		else
			transform.position = toFollow.position + offset;
	}
}
