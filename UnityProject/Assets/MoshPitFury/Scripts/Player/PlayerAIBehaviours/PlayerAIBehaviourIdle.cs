using UnityEngine;
using System.Collections;

public class PlayerAIBehaviourIdle : PlayerAIBehaviour
{
	public override Vector3 WantedDirection
	{
		get { return Vector3.zero; }
	}
}

