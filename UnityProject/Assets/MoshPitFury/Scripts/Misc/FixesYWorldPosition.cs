using UnityEngine;
using System.Collections;

public class FixesYWorldPosition : MonoBehaviour {

	public float YPos;

	public void Update()
	{
		Vector3 position = this.transform.position;
		position.y = YPos;
		this.transform.position = position;
	}
}
