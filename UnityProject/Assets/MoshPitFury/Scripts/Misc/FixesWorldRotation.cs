using UnityEngine;
using System.Collections;

public class FixesWorldRotation : MonoBehaviour {

	public Vector3 eulerAngles;
	private Quaternion rotation;	

	void Start()
	{
		rotation = Quaternion.Euler(eulerAngles);
	}

	void Update () 
	{
		this.transform.rotation = rotation;
	}
}
