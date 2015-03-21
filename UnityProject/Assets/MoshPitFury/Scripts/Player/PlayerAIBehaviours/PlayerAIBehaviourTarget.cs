using UnityEngine;
using System.Collections;

public class PlayerAIBehaviourTarget : PlayerAIBehaviour
{
	public Transform target;
	private Vector3 direction;
	private NavMeshPath path;

	public void NewPath()
	{
		if (target)
		{
			NavMeshPath newPath = new NavMeshPath();
			if (NavMesh.CalculatePath(this.transform.position, this.target.position, -1, newPath))
			{
				this.path = newPath;
			}
		}
	}

	public void Start()
	{
		NewPath();
	}

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		if (path != null && path.corners != null)
		{
			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.yellow;
			style.fontSize = 50;

			Vector3 previous = this.transform.position;
			int i = 0;
			foreach (Vector3 p in this.path.corners)
			{
				UnityEditor.Handles.Label(p + Vector3.up * 10, i.ToString(), style);
				i++;
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(previous, 0.2f);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(previous, p);
				previous = p;
			}
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(previous, 0.2f);

		}
	}
#endif
	// Faire une sorte d'état pour éviter de calculer le chemin chaque frame !
	public override void OnUpdate()
	{
		base.OnUpdate();
		if (path != null)
		{
			this.direction = path.corners[1] - this.transform.position;
			this.direction.Normalize();
		}
		else
		{
			direction = Vector3.zero;
		}
	}

	public override Vector3 WantedDirection
	{
		get { return direction; }
	}
}
