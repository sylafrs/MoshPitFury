using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAIBehaviourTarget : PlayerAIBehaviour
{
	public List<Transform> targets;
	private Transform target;
	private Vector3 direction;
	private NavMeshPath path;

	public void NewTarget()
	{
		this.target = null;

		if(targets == null || targets.Count == 0)
		{
			return;
		}
		
		this.target = this.targets[0];

		if(targets.Count == 1)
		{
			return;
		}

		float nearestDistance = Vector3.Distance(this.transform.position, this.targets[0].position);
		float distance;
		for(int i = 1; i < this.targets.Count; i++)
		{
			distance = Vector3.Distance(this.transform.position, this.targets[i].position);
			if(distance < nearestDistance)
			{
				nearestDistance = distance;
				this.target = this.targets[i];
			}
		}
	}

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
			Vector3 previous = this.transform.position;
			foreach (Vector3 p in this.path.corners)
			{
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

		NewTarget();
		NewPath();

		if (path != null && path.corners.Length > 1)
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

	public override bool WantToDash
	{
		get { return false; }
	}
}
